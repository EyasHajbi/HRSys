using AutoMapper;
using HRSys.Constants;
using HRSys.DTO.Common;
using HRSys.DTO.Home;
using HRSys.Enum;
using HRSys.Repositories.Generic.Interface;
using HRSys.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Home
{
    public class HomeService : IHomeService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        public HomeService(IUnitOfWork unitOfWork,
                                IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }



        //public async Task<(IList<HomeResultDTO> HomeResultDTOs, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, string currentUserId, int complaintsType, int? complaintsTypeId, DateTime? complaintDateFr, DateTime? complaintDateTo, Lang CurrentLang)
        //{
        //    string searchBy = (model.search != null) ? model.search.value : null;
        //    int take = model.length;
        //    int skip = model.start;

        //    string sortBy = "";
        //    bool sortDir = true;

        //    if (model.order != null)
        //    {
        //        sortBy = model.columns[model.order[0].column].data;
        //        sortDir = model.order[0].dir.ToLower() == "asc";
        //    }
        //    int filteredCount = 0;
        //    int totalCount = 0;
        //    var result = await ListPagingExtra(searchBy, take, skip, sortBy, sortDir, CurrentLang, currentUserId, complaintsType, complaintsTypeId, complaintDateFr, complaintDateTo);

        //    if (result.complaints == null)
        //        result.complaints = new List<HomeResultDTO>();
        //    else
        //    {
        //        filteredCount = result.filteredResultsCount;
        //        totalCount = result.totalResultsCount;
        //    }
        //    return (result.complaints, filteredCount, totalCount);
        //}

        //private async Task<(IList<HomeResultDTO> complaints, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang, string currentUserId, int complaintsType, int? complaintsTypeId, DateTime? complaintDateFr, DateTime? complaintDateTo)
        //{
        //    try
        //    {
        //        if (String.IsNullOrEmpty(searchBy))
        //        {
        //            sortBy = "Id";
        //            sortDir = true;
        //        }
        //        var data = await _unitOfWork.ComplaintsRepository.GetComplaints(searchBy, take, skip, sortBy, sortDir, currentUserId, false, complaintsType, complaintsTypeId, complaintDateFr, complaintDateTo);
        //        int filteredResultsCount = data.filteredResultsCount;
        //        int totalResultsCount = data.totalResultsCount;




        //        var culture = CultureInfo.CurrentCulture;
        //        var result = data.complaints.Select(a => new HomeResultDTO()
        //        {
        //            Address = a.complaint.Address,
        //            AddressDetails = (a.complaint.AddressDetails == null ? "" : a.complaint.AddressDetails),
        //            App_MobileNo = (a.complaint.App_Users != null ? a.complaint.App_Users.MobileNo : "........."),
        //            App_UserID = (a.complaint.App_Users != null ? a.complaint.App_UserID : 0),
        //            App_UserName = (a.complaint.App_Users != null ? a.complaint.App_Users.FirstName + " " + a.complaint.App_Users.LastName : "........."),
        //            ComplaintDescription = (a.complaint.Description != "" ? a.complaint.Description : "........."),
        //            ComplaintID = a.complaint.Id,
        //            ComplaintTypeID = a.complaint.ComplaintTypeID,
        //            ComplaintTypeName = a.complaint.ComplaintType.DescriptionAr,
        //            IsOpened = a.complaint.IsOpened,
        //            IsReported = a.complaint.IsReported,
        //            Latitude = (a.complaint.Latitude == null ? "" : a.complaint.Latitude),
        //            Longitude = (a.complaint.Longitude == null ? "" : a.complaint.Longitude),
        //            CurrentLatitude = (a.complaint.CurrentLatitude == null ? "" : a.complaint.CurrentLatitude),
        //            CurrentLongitude = (a.complaint.CurrentLongitude == null ? "" : a.complaint.CurrentLongitude),
        //            RefCode = a.complaint.RefCode,
        //            Response = (string.IsNullOrEmpty(a.complaint.Response) ? "لا يوجد رد" : a.complaint.Response),
        //            ComplaintDate = a.complaint.CreatedOn.ToString("dd/MM/yyyy HH:mm tt")
        //        }).ToList();
        //        return (result, filteredResultsCount, totalResultsCount);

        //    }
        //    catch (Exception ex)
        //    {
        //        return (null, 0, 0);
        //    }

        //}

    }
}
