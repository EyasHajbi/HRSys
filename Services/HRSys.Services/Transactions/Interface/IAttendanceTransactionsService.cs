using HRSys.DTO.Common;
using HRSys.DTO.Lookup;
using HRSys.DTO.Transactions;
using HRSys.Enum;
using HRSys.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Transactions
{
    public interface IAttendanceTransactionsService
    {
        Task<AttendanceTransactionsDto> GetBy(AttendanceTransactionsDto attendanceTransactionsDto);
        void Insert(AttendanceTransactionsDto attendanceTransactionsDto);
        Task<List<AttendanceTransactionsDto>> All(bool ignoreDeleted = true, Expression<Func<AttendanceTransactions, bool>> expression=null);
        AttendanceTransactionsDto GetById(int id);
        void Update(AttendanceTransactionsDto attendanceTransactionsDto);
        Task<(IList<AttendanceTransactionsDto> AttendanceTransactions, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
