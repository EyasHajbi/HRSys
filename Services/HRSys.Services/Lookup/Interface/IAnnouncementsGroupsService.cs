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
    public interface IAnnouncementsGroupsService
    {
        Task<AnnouncementsGroupsDto> GetBy(AnnouncementsGroupsDto announcementsGroupsDto);
        void Insert(AnnouncementsGroupsDto announcementsGroupsDto);
        Task<List<AnnouncementsGroupsDto>> All(bool ignoreDeleted = true, Expression<Func<AnnouncementsGroups, bool>> expression=null);
        AnnouncementsGroupsDto GetById(int id);
        void Update(AnnouncementsGroupsDto announcementsGroupsDto);
        Task<(IList<AnnouncementsGroupsDto> AnnouncementsGroups, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
