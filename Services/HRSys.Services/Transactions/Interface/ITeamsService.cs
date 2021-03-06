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
    public interface ITeamsService
    {
        Task<TeamsDto> GetBy(TeamsDto teamsDto);
        void Insert(TeamsDto teamsDto);
        Task<List<TeamsDto>> All(bool ignoreDeleted = true, Expression<Func<Teams, bool>> expression=null);
        TeamsDto GetById(int id);
        void Update(TeamsDto teamsDto);
        Task<(IList<TeamsDto> Teams, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
