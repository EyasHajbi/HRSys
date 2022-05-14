using HRSys.DTO.Common;
using HRSys.DTO.App_Users;
using HRSys.Enum;
using HRSys.Model;
using _App_Users = HRSys.Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.App_Users
{
    public interface IEmployeesService
    {
        Task<EmployeesDto> GetBy(EmployeesDto app_UsersDto);
        void Insert(EmployeesDto app_UsersDto);
        Task<List<EmployeesDto>> All(bool ignoreDeleted = true, Expression<Func<_App_Users, bool>> expression=null);
        EmployeesDto GetById(int id);
        void Update(EmployeesDto app_UsersDto);
        Task<(IList<EmployeesDto> App_Users, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
        Task<bool> CheckAllowDelete(int Id);
        Task<bool> CheckIsNationalIdExist(string MobileNo);
        Task<bool> CheckIsEmailExist(string Email);
    }
}
