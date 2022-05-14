using HRSys.DTO.Common;
using HRSys.DTO.Lookup;
using HRSys.Enum;
using HRSys.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Lookup
{
    public interface IShiftsService
    {
        Task<ShiftsDto> GetBy(ShiftsDto shiftsDto);
        void Insert(ShiftsDto shiftsDto);
        Task<List<ShiftsDto>> All(bool ignoreDeleted = true, Expression<Func<Shifts, bool>> expression=null);
        ShiftsDto GetById(int id);
        void Update(ShiftsDto shiftsDto);
        Task<(IList<ShiftsDto> Shifts, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
