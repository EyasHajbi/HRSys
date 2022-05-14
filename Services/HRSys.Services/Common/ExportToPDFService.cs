using HRSys.DTO;
using HRSys.Services.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO.Packaging;
using HRSys.Services.SystemSettings;
using System.IO;
using System.Xml;
using System.Linq;
//using word = Microsoft.Office.Interop.Word;
using Word = Aspose.Words;
using PDF =  Aspose.Pdf;
namespace HRSys.Services.Common
{
    public class ExportToPDFService : IExportToPDFService
    {
        private readonly ISystemSettingsSerivce _systemSettingsSerivce;
        public ExportToPDFService(ISystemSettingsSerivce systemSettingsSerivce)
        {
            _systemSettingsSerivce = systemSettingsSerivce;
        }
        public async Task<string> ExportToPDF(ExportToPDFDto exportToPDFDto)
        {
            Package Pack = null;
            string folderPath = "";
            string wordFilePath = "";
            try
            {
                folderPath = await _systemSettingsSerivce.GetSettingValue((int)Enum.SystemSettingsEnum.AttachmentsRootFolder, exportToPDFDto.TenantId, "");
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                wordFilePath = Path.Combine(folderPath, $"{exportToPDFDto.FileName}.docx");
                Uri partURI;
                PackagePart part;

                System.IO.File.Copy(exportToPDFDto.TemplatePath, wordFilePath, true);
                Pack = Package.Open(wordFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                partURI = new Uri("/CustomXml/Item1.xml", UriKind.Relative);
                part = Pack.GetPart(partURI);
                Stream objStream = part.GetStream();
                string contents;
                using (StreamReader reader = new StreamReader(objStream))
                {
                    contents = reader.ReadToEnd();
                }
                XmlDocument objDoc = new XmlDocument();
                objDoc.LoadXml(contents);
                XmlNamespaceManager xnm = new XmlNamespaceManager(new NameTable());
                xnm.AddNamespace("Link", "http://link.sa/K2/ServiceBroker/V1");
                var fields = exportToPDFDto.Fields.Select((value, index) => new {
                    Index = index +1,
                    Value = value
                });
                foreach (var field in fields)
                {
                    objDoc.SelectSingleNode($"/Link:root/Link:field{field.Index}", xnm).InnerText = field.Value;
                }
                objStream = part.GetStream();
                objStream.SetLength(objDoc.OuterXml.Length);
                using (StreamWriter writer = new StreamWriter(objStream))
                {
                    writer.Write(objDoc.OuterXml);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Pack != null) Pack.Close();
            }
            return SaveAsPDF(folderPath, exportToPDFDto.FileName,wordFilePath);
        }

        private string SaveAsPDF(string folderPath, string fileName,string wordFilePath)
        {
            string pdffolderPath = Path.Combine(folderPath, "TempPDF");
            if (!System.IO.Directory.Exists(pdffolderPath)) 
                System.IO.Directory.CreateDirectory(pdffolderPath);
            string pdfFileName = Path.Combine(pdffolderPath, $"{fileName}.pdf");
            Word.Document doc = new Word.Document(wordFilePath);
            doc.Save(pdfFileName);
            return pdfFileName;
        }
    }
}
