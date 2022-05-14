using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HRSys.Api.Exception;
using HRSys.Api.Extensions;
using HRSys.Api.Helpers;
using HRSys.DTO.App_Users;
using HRSys.DTO.Common;
using HRSys.Model;
using HRSys.Services.Lookup;
using HRSys.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : Controller
    {
        private readonly IAnnouncementsGroupsService _announcementsGroupsService;
        private readonly ITasksStatusService _tasksStatusService;
        private readonly IAttendanceStatusesService _attendanceStatusesService;
        private readonly IVacationTypesService _vacationTypesService;
        private readonly INotificationsService _notificationsService;
        private readonly IShiftsService _shiftsService;
        private readonly INationalitiesService _nationalitiesService;
        private readonly ICustomSuccess _customSuccess;

        public ServicesController(IAnnouncementsGroupsService announcementsGroupsService,
            ITasksStatusService tasksStatusService
            , IAttendanceStatusesService attendanceStatusesService, IVacationTypesService vacationTypesService
            , INotificationsService notificationsService
            , IShiftsService shiftsService
            , INationalitiesService nationalitiesService, ICustomSuccess customSuccess)
        {
            _announcementsGroupsService = announcementsGroupsService;
            _tasksStatusService = tasksStatusService;
            _attendanceStatusesService = attendanceStatusesService;
            _vacationTypesService = vacationTypesService;
            _notificationsService = notificationsService;
            _shiftsService = shiftsService;
            _nationalitiesService = nationalitiesService;
            _customSuccess = customSuccess;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetNationalities")]
        [HttpGet]
        public async Task<IActionResult> GetNationalities()
        {
            Expression<Func<Nationalities, bool>> expression = (u => u.IsDeleted != true);

            var model = await _nationalitiesService.All(true, expression);

            return Ok(model);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetAnnouncementsGroups")]
        [HttpGet]
        public async Task<IActionResult> GetAnnouncementsGroups()
        {
            Expression<Func<AnnouncementsGroups, bool>> expression = (u => u.IsDeleted != true);
            var model = await _announcementsGroupsService.All(true, expression);

            return Ok(model);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetTasksStatus")]
        [HttpGet]
        public async Task<IActionResult> GetTasksStatus()
        {
            Expression<Func<TasksStatus, bool>> expression = (u => u.IsDeleted == false);
            var model = await _tasksStatusService.All(true, expression);

            return Ok(model);
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetShifts")]
        [HttpGet]
        public async Task<IActionResult> GetShifts()
        {
            Expression<Func<Shifts, bool>> expression = (u => u.IsDeleted == false);
            var model = await _shiftsService.All(true, expression);

            return Ok(model);
        }




        //[Route("AddComplaints")]
        //[HttpPost]
        //public async Task<IActionResult> AddComplaints()
        //{
        //    string lang = this.Request.Headers["language"].ToString();

        //    if (string.IsNullOrEmpty(lang))
        //        lang = "ar";

        //    ComplaintsDto complaint = await this.Request.GetRawBodyStringAsync<ComplaintsDto>();

        //    if (complaint.ComplaintTypeID == -1)
        //    {
        //        Expression<Func<TasksStatus, bool>> expression = (u => u.IsDeleted == false && u.Code == "SOS");
        //        var model = _complaintTypeService.All(true, expression).GetAwaiter().GetResult().FirstOrDefault();

        //        if (model != null)
        //        {
        //            complaint.IsReported = false;
        //            complaint.IsOpened = false;
        //            complaint.ComplaintTypeID = model.Id;
        //            complaint.CurrentLatitude = complaint.Latitude;
        //            complaint.CurrentLongitude = complaint.Longitude;
        //        }
        //    }

        //    complaint.CreatedOn = DateTime.Now;
        //    _complaintsService.Insert(complaint);

        //    var response = _customSuccess.GetSuccessResponse("AddComplaints." + lang, "InsertSuccess." + lang);
        //    return Ok(response);
        //}

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetAttendanceStatuses")]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceStatuses()
        {
            Expression<Func<AttendanceStatuses, bool>> expression = (u => u.IsDeleted == false);
            var models = await _attendanceStatusesService.All(true, expression);

            return Ok(models);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetNotifications")]
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var model = await _notificationsService.All(true);

            return Ok(model);
        }


        [Route("GetAndroidEncrypt")]
        [HttpGet]
        public async Task<IActionResult> GetAndroidEncrypt()
        {
            string result = await this.Request.GetEncryptRawBodyAsync();

            return Ok(new { result = result });
        }

        [Route("GetAndroidEncryptDecrypt")]
        [HttpGet]
        public async Task<IActionResult> GetAndroidEncryptDecrypt()
        {
            string result = await this.Request.GetDecryptRawBodyAsync();

            return Ok(new { result = result });
        }


        [Route("GetEmployeesDtoFromEncrypt")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeesDtoFromEncrypt()
        {
            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            return Ok(user);
        }
    }
}
