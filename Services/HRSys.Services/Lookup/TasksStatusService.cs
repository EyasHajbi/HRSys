using AutoMapper;
using HRSys.DTO.Common;
using HRSys.DTO.Lookup;
using HRSys.Enum;
using HRSys.Model;
using HRSys.Repositories.Generic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HRSys.DTO;
using HRSys.Services.Lookup;

namespace HRSys.Services.Lookup
{
    public class TasksStatusService : ITasksStatusService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public TasksStatusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<TasksStatusDto>> All(bool ignoreDeleted = true, Expression<Func<TasksStatus, bool>> expression = null)
        {
            IEnumerable<TasksStatus> data = await _unitOfWork.TasksStatusRepository.All(expression);
            List<TasksStatusDto> list = _mapper.Map<List<TasksStatusDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            TasksStatus tasksStatus = _unitOfWork.TasksStatusRepository.GetById(Id, true);
            tasksStatus.IsDeleted = true;
            tasksStatus.ModifiedDate = DateTime.Now;
            _unitOfWork.TasksStatusRepository.Update(tasksStatus);
            _unitOfWork.Save();
        }

        public async Task<TasksStatusDto> GetBy(TasksStatusDto tasksStatusDto)
        {
            Expression<Func<TasksStatus, bool>> expression = (
                   l => (tasksStatusDto.Id == 0 || l.Id == tasksStatusDto.Id) &&
                       (tasksStatusDto.DescriptionAr == "" || l.DescriptionAr.Contains(tasksStatusDto.DescriptionAr)
                       || tasksStatusDto.DescriptionEn == "" || l.DescriptionEn.Contains(tasksStatusDto.DescriptionEn)));

            TasksStatus data = await _unitOfWork.TasksStatusRepository.GetBy(expression);
            TasksStatusDto mapperData = _mapper.Map<TasksStatusDto>(data);
            return mapperData;
        }

        public TasksStatusDto GetById(int id)
        {
            TasksStatus tasksStatus = _unitOfWork.TasksStatusRepository.GetById(id);
            TasksStatusDto tasksStatusDto = _mapper.Map<TasksStatusDto>(tasksStatus);
            return tasksStatusDto;
        }

        public void Insert(TasksStatusDto tasksStatusDto)
        {
            TasksStatus tasksStatus = _mapper.Map<TasksStatus>(tasksStatusDto);
            _unitOfWork.TasksStatusRepository.Add(tasksStatus);
            _unitOfWork.Save();
        }

        public async Task<(IList<TasksStatusDto> TasksStatus, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            int take = model.length;
            int skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }
            int filteredCount = 0;
            int totalCount = 0;
            var result = await ListPagingExtra(searchBy, take, skip, sortBy, sortDir, CurrentLang);

            if (result.tasksStatus == null)
                result.tasksStatus = new List<TasksStatusDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.tasksStatus, filteredCount, totalCount);
        }
        private async Task<(IList<TasksStatusDto> tasksStatus, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<TasksStatus> data = await _unitOfWork.TasksStatusRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<TasksStatusDto> result = _mapper.Map<List<TasksStatusDto>>(data);

            int filteredResultsCount = _unitOfWork.TasksStatusRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.TasksStatusRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<TasksStatus, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<TasksStatus, bool>> expression = (a => a.IsDeleted != true && a.Code != "SOS");
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true && a.Code != "SOS") && (a.DescriptionAr.Contains(searchFilter) || a.DescriptionEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(TasksStatusDto tasksStatusDto)
        {
            tasksStatusDto.ToUpdatable();
            TasksStatus tasksStatus = _unitOfWork.TasksStatusRepository.GetById(tasksStatusDto.Id, true);
            _mapper.Map<TasksStatusDto, TasksStatus>(tasksStatusDto, tasksStatus);

            _unitOfWork.TasksStatusRepository.Update(tasksStatus);
            _unitOfWork.Save();
        }
    }
}
