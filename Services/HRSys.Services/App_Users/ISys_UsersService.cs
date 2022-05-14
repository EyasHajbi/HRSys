using HRSys.DTO.Common;
using HRSys.DTO.App_Users;
using HRSys.Enum;
using HRSys.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HRSys.DTO.Users;

namespace HRSys.Services.App_Users
{
    public interface ISys_UsersService
    {
        Task<Sys_UsersDto> GetByApplicationUserId(string applicationUserId);
        Task<Sys_UsersDto> GetBy(Sys_UsersDto Sys_UsersDto);
        Task<bool> Insert(Sys_UsersDto Sys_UsersDto);
        Task<List<Sys_UsersDto>> All(bool ignoreDeleted = true, Expression<Func<Sys_Users, bool>> expression=null);
        Sys_UsersDto GetById(int id);
        Task<bool> Update(Sys_UsersDto sys_UsersDto);
        Task<(IList<Sys_UsersDto> sys_Users, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
        Task<bool> CheckAllowDelete(int Id);
        Task<bool> ChangePassword(ChangePasswordDto changePasswordDto);
    }
}
