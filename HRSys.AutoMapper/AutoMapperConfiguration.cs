using AutoMapper;
using HRSys.DTO.Lookup;
using HRSys.DTO.SystemSettings;
using HRSys.Shared;
using HRSys.Model;
using System;
using BusinessRoleNamesDto = HRSys.DTO.Lookup.BusinessRoleNamesDto;
using System.Collections.Generic;
using HRSys.DTO.App_Users;
using HRSys.DTO.Transactions;

namespace HRSys.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            ConfigureDTO();
            ConfigureViewModel();
        }
        private void ConfigureDTO()
        {

            CreateMap<BusinessRoleNames, BusinessRoleNamesDto>()
 .ReverseMap()
 .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
 .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<Shifts, ShiftsDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<VacationTypes, VacationTypesDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<TasksStatus, TasksStatusDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            //Users
            CreateMap<Employees, EmployeesDto>()
.ReverseMap()
.ForMember(entity => entity.LastLoginDate, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());


            CreateMap<Sys_Users, Sys_UsersDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.ApplicationUserId, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            //Lookups

            CreateMap<AnnouncementsGroups, AnnouncementsGroupsDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<Notifications, NotificationsDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.DateOfStatus, opt => opt.OnlyIfNotUpdatable());


            CreateMap<AttendanceStatuses, AttendanceStatusesDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<AttendanceTransactions, AttendanceTransactionsDto>()
.ReverseMap();


            CreateMap<VacationsTransactions, VacationsTransactionsDto>()
.ReverseMap();

            CreateMap<Teams, TeamsDto>()
.ReverseMap();

            CreateMap<TeamMembers, TeamMembersDto>()
.ReverseMap();

            CreateMap<AttendanceStatuses, AttendanceStatusesDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());


            CreateMap<Announcements, AnnouncementsDto>()
.ReverseMap();


            CreateMap<BusinessRoles, BusinessRolesDto>()
.ReverseMap()
.ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
.ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<Tasks, TasksDto>()
.ReverseMap();


            CreateMap<Nationalities, NationalitiesDto>()
.ReverseMap();
        }
        private void ConfigureViewModel()
        {

        }
    }
}
