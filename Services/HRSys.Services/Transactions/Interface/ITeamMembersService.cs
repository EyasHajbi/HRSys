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
    public interface ITeamMembersService
    {
        Task<TeamMembersDto> GetBy(TeamMembersDto teamMembersDto);
        void Insert(TeamMembersDto teamMembersDto);
        Task<List<TeamMembersDto>> All(bool ignoreDeleted = true, Expression<Func<TeamMembers, bool>> expression=null);
        TeamMembersDto GetById(int id);
        void Update(TeamMembersDto teamMembersDto);
        Task<(IList<TeamMembersDto> TeamMembers, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
