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
    public interface INationalitiesService
    {
        Task<NationalitiesDto> GetBy(NationalitiesDto nationalitiesDto);
        void Insert(NationalitiesDto nationalitiesDto);
        Task<List<NationalitiesDto>> All(bool ignoreDeleted = true, Expression<Func<Nationalities, bool>> expression=null);
        NationalitiesDto GetById(int id);
        void Update(NationalitiesDto nationalitiesDto);
        Task<(IList<NationalitiesDto> Nationalities, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
