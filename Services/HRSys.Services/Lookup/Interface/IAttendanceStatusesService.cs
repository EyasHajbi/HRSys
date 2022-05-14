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
    public interface IAttendanceStatusesService
    {
        Task<AttendanceStatusesDto> GetBy(AttendanceStatusesDto attendanceStatusesDto);
        void Insert(AttendanceStatusesDto announcementsGroupsDto);
        Task<List<AttendanceStatusesDto>> All(bool ignoreDeleted = true, Expression<Func<AttendanceStatuses, bool>> expression=null);
        AttendanceStatusesDto GetById(int id);
        void Update(AttendanceStatusesDto announcementsGroupsDto);
        Task<(IList<AttendanceStatusesDto> AttendanceStatuses, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
