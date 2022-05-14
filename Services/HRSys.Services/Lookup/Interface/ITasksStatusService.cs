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
    public interface ITasksStatusService
    {
        Task<TasksStatusDto> GetBy(TasksStatusDto tasksStatusDto);
        void Insert(TasksStatusDto tasksStatusDto);
        Task<List<TasksStatusDto>> All(bool ignoreDeleted = true, Expression<Func<TasksStatus, bool>> expression=null);
        TasksStatusDto GetById(int id);
        void Update(TasksStatusDto tasksStatusDto);
        Task<(IList<TasksStatusDto> TasksStatus, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang);
        void Delete(int Id);
    }
}
