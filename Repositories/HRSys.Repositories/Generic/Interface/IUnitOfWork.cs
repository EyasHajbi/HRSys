using HRSys.Model;
using HRSys.Repositories.Transactions.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace HRSys.Repositories.Generic.Interface
{
    public interface IUnitOfWork
    {
        //IGenericRepository<ExternalEntities> ExternalEntitiesRepository { get; }
        //IGenericRepository<InternalEntities> InternalEntitiesRepository { get; }
        //IGenericRepository<AttachmentCategories> AttachmentCategoryRepository { get; }

       
        IGenericRepository<BusinessRoles> BusinessRolesRepository { get; }
        IGenericRepository<BusinessRoleNames> BusinessRoleNamesRepository { get; }
        IGenericRepository<BusinessPermissions> BusinessPermissionsRepository { get; }
        IGenericRepository<ApplicationUser> ApplicationUserRepository { get; }
        IGenericRepository<Teams> TeamsRepository { get; }
        IGenericRepository<VacationTypes> VacationTypesRepository { get; }
        IGenericRepository<AttendanceStatuses> AttendanceStatusesRepository { get; }
        IGenericRepository<Notifications> NotificationsRepository { get; }
        IGenericRepository<TasksStatus> TasksStatusRepository { get; }
        IVacationsTransactionsRepository VacationsTransactionsRepository { get; }
        IGenericRepository<Shifts> ShiftsRepository { get; }
        IGenericRepository<Employees> EmployeesRepository { get; }
        IGenericRepository<Sys_Users> Sys_UsersRepository { get; }
        IAnnouncementsRepository AnnouncementsRepository { get; }
        IGenericRepository<AnnouncementsGroups> AnnouncementsGroupsRepository { get; }
        IGenericRepository<TeamMembers> TeamMembersRepository { get; } 
        IGenericRepository<Nationalities> NationalitiesRepository { get; }
        ITasksRepository TasksRepository { get; }
        IAttendanceTransactionsRepository AttendanceTransactionsRepository { get; }
        void Save();
    }
}
