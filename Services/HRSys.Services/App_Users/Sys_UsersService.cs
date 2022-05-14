using AutoMapper;
using HRSys.DTO.Common;
using HRSys.DTO.Lookup;
using HRSys.Enum;
using HRSys.Model;
using HRSys.Repositories.Generic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HRSys.DTO.App_Users;
using Microsoft.AspNetCore.Identity;
using HRSys.DTO.Users;
using Aspose.Pdf.Operators;

namespace HRSys.Services.App_Users
{
    public class Sys_UsersService : ISys_UsersService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion
        public Sys_UsersService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<Sys_UsersDto>> All(bool ignoreDeleted = true, Expression<Func<Sys_Users, bool>> expression = null)
        {
            IEnumerable<Sys_Users> data = await _unitOfWork.Sys_UsersRepository.All(expression);
            List<Sys_UsersDto> list = _mapper.Map<List<Sys_UsersDto>>(data);
            return list;
        }

        public async Task<Sys_UsersDto> GetByApplicationUserId(string applicationUserId)
        {
            Model.Sys_Users user = await _unitOfWork.Sys_UsersRepository.GetBy(m => m.ApplicationUserId.Trim().ToLower() == applicationUserId.Trim().ToLower());
            Sys_UsersDto mapperData = _mapper.Map<Sys_UsersDto>(user);
            return mapperData;
        }
        public async Task<bool> CheckAllowDelete(int Id)
        {
            bool allow = true;

            //allow = await _unitOfWork.TasksRepository.Any(x => x.PriorityLevelId == Id);
            return !allow;
        }

        public void Delete(int Id)
        {
            Sys_Users _sys__Users = _unitOfWork.Sys_UsersRepository.GetById(Id, true);
            _sys__Users.IsDeleted = true;
            _sys__Users.ModifiedDate = DateTime.Now;
            _unitOfWork.Sys_UsersRepository.Update(_sys__Users);
            _unitOfWork.Save();
        }

        public async Task<Sys_UsersDto> GetBy(Sys_UsersDto sys_UsersDto)
        {
            Expression<Func<Sys_Users, bool>> expression = (
                   l => (sys_UsersDto.Id == 0 || l.Id == sys_UsersDto.Id) &&
                       (sys_UsersDto.FirstName == "" || l.FirstName.Contains(sys_UsersDto.FirstName)));
            Sys_Users data = await _unitOfWork.Sys_UsersRepository.GetBy(expression);
            Sys_UsersDto mapperData = _mapper.Map<Sys_UsersDto>(data);
            return mapperData;
        }

        public Sys_UsersDto GetById(int id)
        {
            Sys_Users sys_Users = _unitOfWork.Sys_UsersRepository.GetById(id);
            Sys_UsersDto sys_UsersDto = _mapper.Map<Sys_UsersDto>(sys_Users);
            return sys_UsersDto;
        }

        public async Task<bool> Insert(Sys_UsersDto sys_UsersDto)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = sys_UsersDto.Email;
            user.Email = sys_UsersDto.Email;
            user.PasswordHash = sys_UsersDto.Password;
            user.AccessFailedCount = 0;
            user.PhoneNumberConfirmed = false;
            user.EmailConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = false;
            var user_ = await _userManager.CreateAsync(user, sys_UsersDto.Password);

            if (!user_.Succeeded)
                return false;

            Sys_Users sys_Users = _mapper.Map<Sys_Users>(sys_UsersDto);
            sys_Users.ApplicationUserId = user.Id;
            _unitOfWork.Sys_UsersRepository.Add(sys_Users);
            _unitOfWork.Save();
            return true;

        }


        public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {

                ApplicationUser appUser = await _userManager.FindByIdAsync(changePasswordDto.UserId);

                if (appUser != null)
                {
                    var _user = await _userManager.ChangePasswordAsync(appUser, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

                    return _user.Succeeded;

                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<(IList<Sys_UsersDto> sys_Users, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.Sys_Users == null)
                result.Sys_Users = new List<Sys_UsersDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.Sys_Users, filteredCount, totalCount);
        }
        private async Task<(IList<Sys_UsersDto> Sys_Users, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<Sys_Users> data = await _unitOfWork.Sys_UsersRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<Sys_UsersDto> result = _mapper.Map<List<Sys_UsersDto>>(data);

            int filteredResultsCount = _unitOfWork.Sys_UsersRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.Sys_UsersRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<Sys_Users, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<Sys_Users, bool>> expression = (a => a.IsDeleted != true);
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true) && (a.FirstName.Contains(searchFilter) || a.SecondName.Contains(searchFilter)));
            }
            return expression;
        }

        public async Task<bool> Update(Sys_UsersDto sys_UsersDto)
        {

            bool isSucceeded = true;

            sys_UsersDto.ToUpdatable();
            Sys_Users sys_Users = _unitOfWork.Sys_UsersRepository.GetById(sys_UsersDto.Id, true);


            ApplicationUser appUser = await _userManager.FindByIdAsync(sys_Users.ApplicationUserId);

            if (appUser != null && sys_UsersDto.Password!=null)
            {
                appUser.Email = sys_UsersDto.Email;
                appUser.PasswordHash=_userManager.PasswordHasher.HashPassword(appUser, sys_UsersDto.Password);

                var _user = await _userManager.UpdateAsync(appUser);

                isSucceeded = _user.Succeeded;

            }

            if (!isSucceeded)
                return isSucceeded;

            _mapper.Map<Sys_UsersDto, Sys_Users>(sys_UsersDto, sys_Users);

            _unitOfWork.Sys_UsersRepository.Update(sys_Users);
            _unitOfWork.Save();

            return true;

        }
    }
}
