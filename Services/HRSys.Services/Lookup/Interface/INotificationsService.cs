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
    public interface INotificationsService
    {
        Task<NotificationsDto> GetBy(NotificationsDto notificationsDto);
        void Insert(NotificationsDto notificationsDto);
        Task<List<NotificationsDto>> All(bool ignoreDeleted = true, Expression<Func<Notifications, bool>> expression=null);
        NotificationsDto GetById(int id);
        void Update(NotificationsDto notificationsDto);
        Task<(IList<NotificationsDto> Notifications, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
