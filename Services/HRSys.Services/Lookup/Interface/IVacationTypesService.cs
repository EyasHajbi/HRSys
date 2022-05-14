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
    public interface IVacationTypesService
    {
        Task<VacationTypesDto> GetBy(VacationTypesDto vacationTypesDto);
        void Insert(VacationTypesDto vacationTypesDto);
        Task<List<VacationTypesDto>> All(bool ignoreDeleted = true, Expression<Func<VacationTypes, bool>> expression=null);
        VacationTypesDto GetById(int id);
        void Update(VacationTypesDto vacationTypesDto);
        Task<(IList<VacationTypesDto> VacationTypes, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
