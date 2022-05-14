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
    public interface IAnnouncementsService
    {
        Task<AnnouncementsDto> GetBy(AnnouncementsDto announcementsDto);
        void Insert(AnnouncementsDto announcementsDto);
        Task<List<AnnouncementsDto>> All(bool ignoreDeleted = true, Expression<Func<Announcements, bool>> expression=null);
        AnnouncementsDto GetById(int id);
        void Update(AnnouncementsDto announcementsDto);
        Task<(IList<AnnouncementsDto> Announcements, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
