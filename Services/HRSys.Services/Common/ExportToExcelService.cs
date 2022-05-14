using HRSys.DTO;
using HRSys.Enum;
using HRSys.Services.Common.Interface;
using HRSys.Services.SystemSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Common
{
    public class ExportToExcelService : IExportToExcelService
    {
        private readonly ISystemSettingsSerivce _systemSettingsSerivce;
        private readonly IHostingEnvironment _hosting;
        private readonly IConfiguration _configuration;
        private int Level = 1;
        private int BaseLevel = 1;
        private int rowIndex = 1;
        private ExcelWorksheet xlsWorksheet;
        private List<ReportColumnsHeaderText> ColumnsHeaders;
        private List<string> IndexesMergeCells;
        private Dictionary<int, int> CollapsedRows = new Dictionary<int, int>();
        public ExportToExcelService(ISystemSettingsSerivce systemSettingsSerivce,
                                    IHostingEnvironment hosting,
                                    IConfiguration configuration)
        {
            this._systemSettingsSerivce = systemSettingsSerivce;
            this._hosting = hosting;
            this._configuration = configuration;


        }
        public async Task<string> ExportToExcel(ExportToExcelDto exportToExcelDto, Enum.Lang lang, OfficeOpenXml.Table.TableStyles tableStyle = TableStyles.None)
        {
            string folderPath = await _systemSettingsSerivce.GetSettingValue((int)Enum.SystemSettingsEnum.AttachmentsRootFolder, exportToExcelDto.TenantId, "");

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, exportToExcelDto.FileName);
            FileInfo targetFile = new FileInfo(filePath);
            using (var excelFile = new ExcelPackage(targetFile))
            {
                var worksheet = excelFile.Workbook.Worksheets.Add("Sheet1");
                MethodInfo method = typeof(ExportToExcelService).GetMethod("CloneListAs");
                MethodInfo genericMethod = method.MakeGenericMethod(exportToExcelDto.Data[0].GetType());
                dynamic dataList = genericMethod.Invoke(null, new[] { exportToExcelDto.Data });
                int index = 1;
                foreach (var item in exportToExcelDto.Columns)
                {
                    worksheet.Cells[1, index++].Value = item;
                }
                MemberInfo[] properties = exportToExcelDto.Data[0].GetType().GetProperties();
                MemberInfo[] membersToInclude = properties
                      .Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute)))
                      .ToArray();


                worksheet.Cells["A2"].LoadFromCollection(Collection: dataList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None, memberFlags: BindingFlags.Instance | BindingFlags.Public, Members: membersToInclude);
                FormatedCells(exportToExcelDto, lang, tableStyle, worksheet, dataList);

                excelFile.Save();
            }
            return filePath;
        }

        private static void FormatedCells(ExportToExcelDto exportToExcelDto, Lang lang, TableStyles tableStyle, ExcelWorksheet worksheet, dynamic dataList)
        {
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            worksheet.Cells.Style.WrapText = true;
            worksheet.View.RightToLeft = lang == Enum.Lang.ar ? true : false;
            worksheet.Cells.Style.HorizontalAlignment = lang == Enum.Lang.ar ? OfficeOpenXml.Style.ExcelHorizontalAlignment.Right : OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            var range = worksheet.Cells["A1:" + OfficeOpenXml.ExcelCellAddress.GetColumnLetter(exportToExcelDto.Columns.Count()) + (dataList.Count + 1).ToString()];
            OfficeOpenXml.Table.ExcelTable table = worksheet.Tables.Add(range, "table1");
            table.ShowFilter = false;
            table.TableStyle = tableStyle;
        }

        public static List<T> CloneListAs<T>(IList<object> source)
        {
            return source.Cast<T>().ToList();
        }
        public static List<T> CloneListAs1<T>(dynamic source)
        {
            return source.Cast<T>().ToList();
        }
        public static bool CheckIsList(dynamic source)
        {
            try
            {
                var value = source[0];
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<string> ExportToExcelMasterDetails(ExportToExcelMasterDetailsDto exportToExcelMasterDetailsDto, Lang lang, string headerCellStyle, string detailsCellstyle, string templateName = "Template.xlsx", TableStyles tableStyle = TableStyles.None)
        {
            string folderPath = await _systemSettingsSerivce.GetSettingValue((int)Enum.SystemSettingsEnum.AttachmentsRootFolder, exportToExcelMasterDetailsDto.TenantId, "");
            dynamic dataList = null;
            List<string> indexes = new List<string>();
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);
            var templatePath = Path.Combine(_hosting.WebRootPath, _configuration["ApplicationSettings:MasterTemplateExcelPath"], templateName);
            string filePath = Path.Combine(folderPath, exportToExcelMasterDetailsDto.FileName);
            FileInfo targetFile = new FileInfo(templatePath);
            int countRows = 1;
            int cellIndexMaster = 2;
            int cellIndexDetails = 4;
            int mergeEmptyDetailsColumnsData = 0;
            int mergeEmptySubDetailsColumnsData = 0;

            int rangeCount = (exportToExcelMasterDetailsDto.ColumnsMaster.Count > exportToExcelMasterDetailsDto.ColumnsDetails.Count ?
                                   exportToExcelMasterDetailsDto.ColumnsMaster.Count : exportToExcelMasterDetailsDto.ColumnsDetails.Count);



            using (var excelFile = new ExcelPackage(targetFile))
            {

                var worksheet = excelFile.Workbook.Worksheets["Sheet1"];
                MethodInfo method = typeof(ExportToExcelService).GetMethod("CloneListAs");
                if (exportToExcelMasterDetailsDto.Report != null)
                {


                    foreach (var item in exportToExcelMasterDetailsDto.Report)
                    {

                        int IndexMaster = 1;
                        foreach (var master in exportToExcelMasterDetailsDto.ColumnsMaster)
                        {
                            int index1 = IndexMaster++;
                            worksheet.Cells[countRows, index1].Value = master;
                            worksheet.Cells[countRows, index1].StyleName = detailsCellstyle;
                        }
                        string cellTo1 = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(rangeCount) + countRows;

                        if (exportToExcelMasterDetailsDto.ColumnsMaster.Count < exportToExcelMasterDetailsDto.ColumnsDetails.Count)
                            indexes.Add("A" + countRows + ":" + cellTo1);

                        countRows++;

                        if (item.Data != null)
                        {
                            MethodInfo genericMethod = method.MakeGenericMethod(item.Data.GetType());
                            MemberInfo[] properties = item.Data.GetType().GetProperties();
                            dataList = new List<object>();
                            dataList.Add(item.Data);

                            MemberInfo[] membersToInclude = properties
                                  .Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute)))
                                  .ToArray();
                            worksheet.Cells["A" + countRows].LoadFromCollection(Collection: dataList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None, memberFlags: BindingFlags.Instance | BindingFlags.Public, Members: membersToInclude);


                            if (exportToExcelMasterDetailsDto.ColumnsDetails.Count != exportToExcelMasterDetailsDto.ColumnsMaster.Count)
                            {

                                string cellTo = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(rangeCount) + countRows;

                                if (exportToExcelMasterDetailsDto.ColumnsMaster.Count < exportToExcelMasterDetailsDto.ColumnsDetails.Count)
                                    indexes.Add("A" + countRows + ":" + cellTo);

                                worksheet.Cells["A" + countRows + ":" + cellTo].StyleName = headerCellStyle;
                            }

                            countRows++;

                            try
                            {
                                if (item.Details != null && item.Details.Data != null)
                                {
                                    countRows++;

                                    if (item.Details.Data.Count == 0)
                                    {
                                        mergeEmptyDetailsColumnsData = countRows + 1;
                                        string cellToDerails = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(exportToExcelMasterDetailsDto.ColumnsDetails.Count()) + mergeEmptyDetailsColumnsData;
                                        item.Details.Data.Add(
                                            "لا يوجد "
                                        );
                                        indexes.Add("A" + mergeEmptyDetailsColumnsData + ":" + cellToDerails);
                                    }

                                    genericMethod = method.MakeGenericMethod(item.Details.Data[0].GetType());
                                    dataList = genericMethod.Invoke(null, new[] { item.Details.Data });
                                    int indexDetails = 1;
                                    foreach (var detail in exportToExcelMasterDetailsDto.ColumnsDetails)
                                    {
                                        int index1 = indexDetails++;
                                        worksheet.Cells[countRows, index1].Value = detail;
                                        worksheet.Cells[countRows, index1].StyleName = detailsCellstyle;
                                    }
                                    countRows++;

                                    properties = item.Details.Data[0].GetType().GetProperties();
                                    membersToInclude = properties
                                          .Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute)))
                                          .ToArray();


                                    worksheet.Cells["A" + countRows].LoadFromCollection(Collection: dataList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None, memberFlags: BindingFlags.Instance | BindingFlags.Public, Members: membersToInclude);
                                    countRows += dataList.Count;

                                }
                                if (item.SubDetails != null && item.SubDetails.Data != null)
                                {
                                    countRows++;
                                    if (item.SubDetails.Data.Count == 0)
                                    {
                                        mergeEmptySubDetailsColumnsData = countRows + 1;
                                        string cellToDerails = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(exportToExcelMasterDetailsDto.ColumnsSubDetails.Count()) + mergeEmptySubDetailsColumnsData;
                                        item.SubDetails.Data.Add(
                                            "لا يوجد "
                                        );
                                        indexes.Add("A" + mergeEmptySubDetailsColumnsData + ":" + cellToDerails);
                                    }
                                    genericMethod = method.MakeGenericMethod(item.SubDetails.Data[0].GetType());
                                    dataList = genericMethod.Invoke(null, new[] { item.SubDetails.Data });
                                    int indexSubDetails = 1;
                                    foreach (var detail in exportToExcelMasterDetailsDto.ColumnsSubDetails)
                                    {
                                        int index1 = indexSubDetails++;
                                        worksheet.Cells[countRows, index1].Value = detail;
                                        worksheet.Cells[countRows, index1].StyleName = detailsCellstyle;
                                    }
                                    countRows++;

                                    properties = item.SubDetails.Data[0].GetType().GetProperties();
                                    membersToInclude = properties
                                          .Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute)))
                                          .ToArray();


                                    worksheet.Cells["A" + countRows].LoadFromCollection(Collection: dataList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None, memberFlags: BindingFlags.Instance | BindingFlags.Public, Members: membersToInclude);
                                    countRows += dataList.Count;

                                }
                                cellIndexMaster += countRows + 1;
                                // cellIndexDetails += cellIndexMaster + 2;

                                countRows++;
                            }
                            catch (Exception ex)
                            {
                                cellIndexMaster = countRows + 1;
                                cellIndexDetails = cellIndexMaster + 2;
                            }
                        }

                    }
                }
                FileInfo saveFileInfo = new FileInfo(filePath);
                FormatedCellsMasterDetails(exportToExcelMasterDetailsDto, lang, tableStyle, worksheet, countRows, indexes);
                excelFile.SaveAs(saveFileInfo);
            }
            return filePath;
        }



        private static void FormatedCellsMasterDetails(ExportToExcelMasterDetailsDto exportToExcelMasterDetailsDto, Lang lang, TableStyles tableStyle, ExcelWorksheet worksheet, int count, List<string> indexesMergeCells)
        {
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            worksheet.Cells.Style.WrapText = true;
            worksheet.View.RightToLeft = lang == Enum.Lang.ar ? true : false;
            worksheet.Cells.Style.HorizontalAlignment = lang == Enum.Lang.ar ? OfficeOpenXml.Style.ExcelHorizontalAlignment.Right : OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


            var range = worksheet.Cells["A1:" + OfficeOpenXml.ExcelCellAddress.GetColumnLetter(exportToExcelMasterDetailsDto.ColumnsDetails.Count()) + (count).ToString()];

            if (indexesMergeCells != null && indexesMergeCells.Count > 0)
            {
                foreach (var item in indexesMergeCells)
                {
                    worksheet.Cells[item].Merge = true;
                    worksheet.Cells[item].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

            }
            worksheet.Cells["AA1"].StyleName = "";
            worksheet.Cells["AA2"].StyleName = "";
            worksheet.Cells["AA3"].StyleName = "";
            worksheet.Cells["AA4"].StyleName = "";

        }

        public async Task<string> ExportToExcelMasterDetails(GenericReport report, Lang lang)
        {
            try
            {
                MethodInfo methodInfo = typeof(ExportToExcelService).GetMethod("CheckIsList");
                bool isList = (bool)methodInfo.Invoke(null, new[] { report.Data });
                dynamic dataList = null;
                if (string.IsNullOrWhiteSpace(report.TemplateName)) report.TemplateName = "Template.xlsx";
                if (isList)
                {
                    methodInfo = typeof(ExportToExcelService).GetMethod("CloneListAs");
                    MethodInfo genericMethod = methodInfo.MakeGenericMethod(report.Data[0].GetType());
                    dataList = genericMethod.Invoke(null, new[] { report.Data });
                }
                string folderPath = await _systemSettingsSerivce.GetSettingValue((int)Enum.SystemSettingsEnum.AttachmentsRootFolder, 1, "");

                List<string> indexes = new List<string>();
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);
                var templatePath = Path.Combine(_hosting.WebRootPath, _configuration["ApplicationSettings:MasterTemplateExcelPath"], report.TemplateName);
                string filePath = Path.Combine(folderPath, report.FileName);
                FileInfo targetFile = new FileInfo(templatePath);
                rowIndex = 1;
                using (var excelFile = new ExcelPackage(targetFile))
                {
                    xlsWorksheet = excelFile.Workbook.Worksheets["Sheet1"];
                    this.ColumnsHeaders = report.ColumnsHeaders;
                    if (isList)
                    {
                        foreach (var item in dataList)
                        {
                            this.Level = 1;
                            dataList = LoadMaster(item);
                            rowIndex++;
                        }

                    }
                    FormatedCellsMasterDetails(lang);
                    FileInfo saveFileInfo = new FileInfo(filePath);
                    excelFile.SaveAs(saveFileInfo);
                    return filePath;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private dynamic LoadMaster(dynamic item)
        {
            int currentIndex = this.rowIndex;
            dynamic dataList;
            MemberInfo[] properties = item.GetType().GetProperties();
            properties = properties.Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute))).ToArray();
            dataList = new List<object>();
            dataList.Add(item);
            List<MemberInfo> listProperties = new List<MemberInfo>();
            int pCount = 0;
            foreach (var property in properties.ToList())
            {
                var value = item.GetType().GetProperty(property.Name).GetValue(item, null);
                if (value == null) continue;
                var type = value.GetType();
                if (type.IsGenericType)
                {
                    listProperties.Add(property);
                }
                else
                    pCount++;
            }
            this.Level = 1;
            DrawColumnsHeader();
            MemberInfo[] membersToInclude = properties
                  .Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute)) && !listProperties.Contains(p))
                  .ToArray();

            this.xlsWorksheet.Cells["A" + this.rowIndex].LoadFromCollection(Collection: dataList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None, memberFlags: BindingFlags.Instance | BindingFlags.Public, Members: membersToInclude);
            CheckNeedMerge(pCount);


            this.rowIndex++;
            if (listProperties.Count > 0)
            {
                this.Level++;
                LoadDetails(listProperties, item);
            }
            if (!CollapsedRows.ContainsKey(currentIndex))
                CollapsedRows.Add(currentIndex, this.rowIndex);
            return dataList;
        }

        private void CheckNeedMerge(int columnCount)
        {
            if (this.ColumnsHeaders == null || this.ColumnsHeaders.Count == 0) return;
            List<ReportColumnsHeaderText> headers = this.ColumnsHeaders.Where(a => a.ColumnLevel == this.Level).ToList();
            if (headers == null || headers.Count == 0) return;
            if (this.IndexesMergeCells == null) this.IndexesMergeCells = new List<string>();
            var grouped = this.ColumnsHeaders.GroupBy(a => a.ColumnLevel);
            int maxCount = grouped.Max(a => a.Count());

            if (headers.Count > maxCount)
                maxCount = headers.Count;

            if (columnCount > maxCount)
                maxCount = columnCount;
            string columnLetter = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(maxCount);
            string currentColumnLetter = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(headers.Count);

            var range = this.xlsWorksheet.Cells["A" + (this.rowIndex - 1).ToString() + ":" + columnLetter + (this.rowIndex - 1).ToString()];
            if (headers.Count < maxCount)
            {
                this.IndexesMergeCells.Add(currentColumnLetter + (this.rowIndex - 1).ToString() + ":" + columnLetter + (this.rowIndex - 1).ToString());
            }
        }

        private void DrawColumnsHeader()
        {
            if (this.ColumnsHeaders == null || this.ColumnsHeaders.Count == 0) return;
            List<ReportColumnsHeaderText> headers = this.ColumnsHeaders.Where(a => a.ColumnLevel == this.Level).ToList();
            if (headers == null || headers.Count == 0) return;
            int colIndex = 1;

            foreach (var item in headers)
            {
                this.xlsWorksheet.Cells[this.rowIndex, colIndex].Value = item.Text;
                this.xlsWorksheet.Cells[this.rowIndex, colIndex].StyleName = item.CellStyleName;
                colIndex++;
            }
            this.rowIndex++;
        }

        private List<MemberInfo> CheckChildren(dynamic value)
        {
            List<MemberInfo> listProperties = new List<MemberInfo>();
            foreach (var v in value)
            {
                dynamic dataList;
                MemberInfo[] properties = v.GetType().GetProperties();
                properties = properties.Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute))).ToArray();
                dataList = new List<object>();
                dataList.Add(v);

                int pCount = 0;
                foreach (var property1 in properties)
                {
                    if (property1.GetType().IsValueType) continue;
                    var value1 = v.GetType().GetProperty(property1.Name).GetValue(v, null);
                    if (value1 == null) continue;
                    var type1 = value1.GetType();
                    if (type1.IsValueType) break;
                    if (type1.IsGenericType && !listProperties.Contains(property1))
                    {

                        listProperties.Add(property1);
                    }
                    else
                        pCount++;
                }
            }
            return listProperties;
        }
        private void LoadDetails(List<MemberInfo> listProperties, dynamic item)
        {
            int currentIndex = this.rowIndex;




            foreach (var property in listProperties)
            {
                var value = item.GetType().GetProperty(property.Name).GetValue(item, null);
                if (value == null) continue;
                var type = value.GetType();
                var dataList = new List<object>(); ;
                if (type.IsGenericType)
                {
                    List<MemberInfo> listProperties1 = CheckChildren(value);
                    if (listProperties1.Count == 0)
                    {
                        dataList = new List<object>();
                        MemberInfo[] propertiesBase;
                        if (value == null || value.Count == 0)
                        {
                            dataList.Add("لا يوجد");

                        }
                        else
                        {
                            dataList.AddRange(value);
                        }
                        propertiesBase = dataList[0].GetType().GetProperties();
                        DrawData(currentIndex, dataList, listProperties1, value, propertiesBase.ToList(), 0);
                    }
                    else
                    {
                        foreach (var v in value)
                        {
                            dataList = new List<object>();
                            MemberInfo[] properties = v.GetType().GetProperties();
                            dataList.Add(v);

                            int pCount = 0;

                            pCount = properties.ToList().Count(x => !listProperties1.Contains(x));
                            DrawData(currentIndex, dataList, listProperties1, v, properties.ToList(), pCount);
                        }
                    }

                    string dd = "";
                }
            }
            this.Level--;
        }

        private void DrawData(int currentIndex, List<object> dataList, List<MemberInfo> listProperties1, dynamic v, List<MemberInfo> properties, int pCount)
        {
            MemberInfo[] membersToInclude = properties
                  .Where(p => !Attribute.IsDefined(p, typeof(HRSys.DTO.EpplusIgnoreAttribute)) && !listProperties1.Contains(p))
                  .ToArray();
            DrawColumnsHeader();
            this.xlsWorksheet.Cells["A" + this.rowIndex].LoadFromCollection(Collection: dataList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None, memberFlags: BindingFlags.Instance | BindingFlags.Public, Members: membersToInclude);
            CheckNeedMerge(pCount);
            this.rowIndex++;
            if (listProperties1.Count > 0)
            {
                this.Level++;
                LoadDetails(listProperties1, v);
                if (CollapsedRows.ContainsKey(currentIndex))
                    CollapsedRows.Add(currentIndex, this.rowIndex);
            }
        }

        private void FormatedCellsMasterDetails(Lang lang)
        {
            this.xlsWorksheet.Cells[this.xlsWorksheet.Dimension.Address].AutoFitColumns();
            this.xlsWorksheet.Cells.Style.WrapText = true;
            this.xlsWorksheet.View.RightToLeft = lang == Enum.Lang.ar ? true : false;
            this.xlsWorksheet.Cells.Style.HorizontalAlignment = lang == Enum.Lang.ar ? OfficeOpenXml.Style.ExcelHorizontalAlignment.Right : OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            this.xlsWorksheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            if (this.IndexesMergeCells != null && this.IndexesMergeCells.Count > 0)
            {
                foreach (var item in this.IndexesMergeCells)
                {
                    this.xlsWorksheet.Cells[item].Merge = true;
                    this.xlsWorksheet.Cells[item].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

            }

            if (this.CollapsedRows != null && this.CollapsedRows.Count > 0)
            {
                int maxLevel = this.ColumnsHeaders.Max(a => a.ColumnLevel);
                if (maxLevel > 2)
                {
                    int max = CollapsedRows.Max(a => a.Value);

                    for (int i = 1; i <= max + CollapsedRows.Count() + 1; i++)
                    {
                        this.xlsWorksheet.Row(i).OutlineLevel = 1;
                        this.xlsWorksheet.Row(i).Collapsed = true;
                    }

                    foreach (var item in CollapsedRows)
                    {
                        this.xlsWorksheet.Row(item.Key).Collapsed = false;
                        this.xlsWorksheet.Row(item.Key).OutlineLevel = item.Key == 1 ? 0 : item.Key;
                        this.xlsWorksheet.Row(item.Key + 1).Collapsed = false;
                        this.xlsWorksheet.Row(item.Key + 1).OutlineLevel = item.Key == 1 ? 0 : item.Key;

                        this.xlsWorksheet.DeleteRow(item.Value);
                        this.xlsWorksheet.InsertRow(item.Value, 1);

                    }
                }
            }

            this.xlsWorksheet.Cells["AA1"].StyleName = "";
            this.xlsWorksheet.Cells["AA2"].StyleName = "";
            this.xlsWorksheet.Cells["AA3"].StyleName = "";
            this.xlsWorksheet.Cells["AA4"].StyleName = "";

        }

    }
}
