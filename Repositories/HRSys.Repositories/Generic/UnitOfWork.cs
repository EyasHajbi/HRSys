using HRSys.Model;
using HRSys.Repositories.Generic.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRSys.Repositories.Transactions.Interface;
using HRSys.Transactions;

namespace HRSys.Repositories.Generic
{
    public class UnitOfWork : IUnitOfWork, System.IDisposable
    {
        #region Fields
        private bool disposed = false;
        private readonly HRSysContext _context;
        //private IGenericRepository<ExternalEntities> _externalEntitiesRepository;
        //private IGenericRepository<InternalEntities> _internalEntitiesRepository;
        //private IGenericRepository<AttachmentCategories> _attachmentCategoryRepository;


        private IGenericRepository<BusinessRoles> _businessRolesRepository;
        private IGenericRepository<BusinessRoleNames> _businessRoleNamesRepository;
        private IGenericRepository<BusinessPermissions> _businessPermissionsRepository;
        private IGenericRepository<ApplicationUser> _applicationUserRepository;
        private IGenericRepository<Teams> _teamsRepository;
        private IGenericRepository<VacationTypes> _vacationTypesRepository;
        private IGenericRepository<AttendanceStatuses> _attendanceStatusesRepository;
        private IGenericRepository<Notifications> _notificationsRepository;
        private IGenericRepository<TasksStatus> _tasksStatusRepository;
        private IVacationsTransactionsRepository _vacationsTransactionsRepository;
        private IGenericRepository<Shifts> _shiftsRepository;
        private IGenericRepository<Employees> _employeesRepository;
        private IGenericRepository<Sys_Users> _sys_UsersRepository;
        private IAnnouncementsRepository _announcementsRepository;
        private IGenericRepository<AnnouncementsGroups> _announcementsGroupsRepository;
        private IGenericRepository<TeamMembers> _teamMembersRepository; 
        private IGenericRepository<Nationalities> _nationalitiesRepository;
        private ITasksRepository _tasksRepository; 
        private IAttendanceTransactionsRepository _attendanceTransactionsRepository;
        #endregion

        #region Properties
        //public IGenericRepository<ExternalEntities> ExternalEntitiesRepository { get => _externalEntitiesRepository ?? (_externalEntitiesRepository = new GenericRepository<ExternalEntities>(_context)); }
        //public IGenericRepository<InternalEntities> InternalEntitiesRepository { get => _internalEntitiesRepository ?? (_internalEntitiesRepository = new GenericRepository<InternalEntities>(_context)); }
        //public IGenericRepository<AttachmentCategories> AttachmentCategoryRepository { get => _attachmentCategoryRepository ?? (_attachmentCategoryRepository = new GenericRepository<AttachmentCategories>(_context)); }
        public IGenericRepository<BusinessRoles> BusinessRolesRepository { get => _businessRolesRepository ?? (_businessRolesRepository = new GenericRepository<BusinessRoles>(_context)); }

        public IGenericRepository<BusinessRoleNames> BusinessRoleNamesRepository { get => _businessRoleNamesRepository ?? (_businessRoleNamesRepository = new GenericRepository<BusinessRoleNames>(_context)); }
        public IGenericRepository<BusinessPermissions> BusinessPermissionsRepository { get => _businessPermissionsRepository ?? (_businessPermissionsRepository = new GenericRepository<BusinessPermissions>(_context)); }
        public IGenericRepository<ApplicationUser> ApplicationUserRepository { get => _applicationUserRepository ?? (_applicationUserRepository = new GenericRepository<ApplicationUser>(_context)); }

        public IGenericRepository<Teams> TeamsRepository { get => _teamsRepository ?? (_teamsRepository = new GenericRepository<Teams>(_context)); }

        public IGenericRepository<VacationTypes> VacationTypesRepository { get => _vacationTypesRepository ?? (_vacationTypesRepository = new GenericRepository<VacationTypes>(_context)); }

        public IGenericRepository<AttendanceStatuses> AttendanceStatusesRepository { get => _attendanceStatusesRepository ?? (_attendanceStatusesRepository = new GenericRepository<AttendanceStatuses>(_context)); }

        public IGenericRepository<Notifications> NotificationsRepository { get => _notificationsRepository ?? (_notificationsRepository = new GenericRepository<Notifications>(_context)); }

        public IGenericRepository<Employees> EmployeesRepository { get => _employeesRepository ?? (_employeesRepository = new GenericRepository<Employees>(_context)); }

        public IGenericRepository<Sys_Users> Sys_UsersRepository { get => _sys_UsersRepository ?? (_sys_UsersRepository = new GenericRepository<Sys_Users>(_context)); }

        public IGenericRepository<TasksStatus> TasksStatusRepository { get => _tasksStatusRepository ?? (_tasksStatusRepository = new GenericRepository<TasksStatus>(_context)); }

        public IVacationsTransactionsRepository VacationsTransactionsRepository { get => _vacationsTransactionsRepository ?? (_vacationsTransactionsRepository = new VacationsTransactionsRepository(_context)); }

        public IAnnouncementsRepository AnnouncementsRepository { get => _announcementsRepository ?? (_announcementsRepository = new AnnouncementsRepository(_context)); }

        public IGenericRepository<AnnouncementsGroups> AnnouncementsGroupsRepository { get => _announcementsGroupsRepository ?? (_announcementsGroupsRepository = new GenericRepository<AnnouncementsGroups>(_context)); }
        public IGenericRepository<Shifts> ShiftsRepository { get => _shiftsRepository ?? (_shiftsRepository = new GenericRepository<Shifts>(_context)); }
        public IGenericRepository<TeamMembers> TeamMembersRepository { get => _teamMembersRepository ?? (_teamMembersRepository = new GenericRepository<TeamMembers>(_context)); }
        public IGenericRepository<Nationalities> NationalitiesRepository { get => _nationalitiesRepository ?? (_nationalitiesRepository = new GenericRepository<Nationalities>(_context)); }
        public ITasksRepository TasksRepository { get => _tasksRepository ?? (_tasksRepository = new TasksRepository(_context)); }
        public IAttendanceTransactionsRepository AttendanceTransactionsRepository { get => _attendanceTransactionsRepository ?? (_attendanceTransactionsRepository = new AttendanceTransactionsRepository(_context)); }

        #endregion
        #region CTOR
        public UnitOfWork(HRSysContext context)
        {
            _context = context;
        }
        #endregion
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
    }
}
