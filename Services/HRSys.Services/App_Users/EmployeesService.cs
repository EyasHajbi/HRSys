using AutoMapper;
using HRSys.DTO.Common;
using HRSys.DTO.Lookup;
using HRSys.Enum;
using HRSys.Model;
using _App_Users = HRSys.Model.Employees;
using HRSys.Repositories.Generic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HRSys.DTO.App_Users;
using Microsoft.AspNetCore.Identity;

namespace HRSys.Services.App_Users
{
    public class EmployeesService : IEmployeesService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion
        public EmployeesService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userManager = userManager;
        }
        public async Task<List<EmployeesDto>> All(bool ignoreDeleted = true, Expression<Func<Employees, bool>> expression = null)
        {
            IEnumerable<Employees> data = await _unitOfWork.EmployeesRepository.All(expression);
            List<EmployeesDto> list = _mapper.Map<List<EmployeesDto>>(data);
            return list;
        }

        public async Task<bool> CheckAllowDelete(int Id)
        {
            bool allow = true;

            //allow = await _unitOfWork.TasksRepository.Any(x => x.PriorityLevelId == Id);
            return !allow;
        }

        public async Task<bool> CheckIsNationalIdExist(string NationalId)
        {
            bool isExist = true;

            isExist = await _unitOfWork.EmployeesRepository.Any(x => x.NationalId == NationalId && x.IsDeleted == false);
            return isExist;
        }

        public async Task<bool> CheckIsEmailExist(string Email)
        {
            bool isExist = true;

          //  isExist = await _unitOfWork.EmployeesRepository.Any(x => x.Email == Email && x.IsDeleted == false);
            return isExist;
        }
        public void Delete(int Id)
        {
            _App_Users _app_Users = _unitOfWork.EmployeesRepository.GetById(Id, true);
            _app_Users.IsDeleted = true;
            _app_Users.ModifiedDate = DateTime.Now;
            _unitOfWork.EmployeesRepository.Update(_app_Users);
            _unitOfWork.Save();
        }

        public async Task<EmployeesDto> GetBy(EmployeesDto app_UsersDto)
        {
            Expression<Func<_App_Users, bool>> expression = (
                   l => (app_UsersDto.Id == 0 || l.Id == app_UsersDto.Id) &&
                       (app_UsersDto.FirstName == "" || l.FirstName.Contains(app_UsersDto.FirstName)));
            _App_Users data = await _unitOfWork.EmployeesRepository.GetBy(expression);
            EmployeesDto mapperData = _mapper.Map<EmployeesDto>(data);
            return mapperData;
        }

        public EmployeesDto GetById(int id)
        {
            Employees app_Users = _unitOfWork.EmployeesRepository.GetById(id);
            EmployeesDto app_UsersDto = _mapper.Map<EmployeesDto>(app_Users);
            return app_UsersDto;
        }

        public void Insert(EmployeesDto app_UsersDto)
        {
            try
            {
                //ApplicationUser user = new ApplicationUser();
                //user.UserName = app_UsersDto.NationalId;
                //user.Email = app_UsersDto.NationalId;
                //user.PasswordHash = app_UsersDto.Password;
                //user.AccessFailedCount = 0;
                //user.PhoneNumberConfirmed = false;
                //user.EmailConfirmed = false;
                //user.TwoFactorEnabled = false;
                //user.LockoutEnabled = false;
                //var user_ =  _userManager.CreateAsync(user, app_UsersDto.Password).GetAwaiter().GetResult();

                //if (!user_.Succeeded)
                //    return ;

                Employees app_Users = _mapper.Map<Employees>(app_UsersDto);
                _unitOfWork.EmployeesRepository.Add(app_Users);
                _unitOfWork.Save();
                app_UsersDto.Id = app_Users.Id;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<(IList<EmployeesDto> App_Users, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            int take = model.length;
            int skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }
            int filteredCount = 0;
            int totalCount = 0;
            var result = await ListPagingExtra(searchBy, take, skip, sortBy, sortDir, CurrentLang);

            if (result.App_Users == null)
                result.App_Users = new List<EmployeesDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.App_Users, filteredCount, totalCount);
        }
        private async Task<(IList<EmployeesDto> App_Users, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<_App_Users> data = await _unitOfWork.EmployeesRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<EmployeesDto> result = _mapper.Map<List<EmployeesDto>>(data);

            int filteredResultsCount = _unitOfWork.EmployeesRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.EmployeesRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<_App_Users, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<_App_Users, bool>> expression = (a => a.IsDeleted != true);
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true) && (a.FirstName.Contains(searchFilter) || a.SecondName.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(EmployeesDto App_UsersDto)
        {
            try
            {

           
            App_UsersDto.ToUpdatable();
            _App_Users _app_Users = _unitOfWork.EmployeesRepository.GetById(App_UsersDto.Id, true);
            _mapper.Map<EmployeesDto, _App_Users>(App_UsersDto, _app_Users);

            _unitOfWork.EmployeesRepository.Update(_app_Users);
            _unitOfWork.Save();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
