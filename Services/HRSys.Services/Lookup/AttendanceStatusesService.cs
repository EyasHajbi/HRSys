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
    public class AttendanceStatusesService : IAttendanceStatusesService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public AttendanceStatusesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<AttendanceStatusesDto>> All(bool ignoreDeleted = true, Expression<Func<AttendanceStatuses, bool>> expression = null)
        {
            IEnumerable<AttendanceStatuses> data = await _unitOfWork.AttendanceStatusesRepository.All(expression);
            List<AttendanceStatusesDto> list = _mapper.Map<List<AttendanceStatusesDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            AttendanceStatuses attendanceStatuses = _unitOfWork.AttendanceStatusesRepository.GetById(Id, true);
            attendanceStatuses.IsDeleted = true;
            attendanceStatuses.ModifiedDate = DateTime.Now;
            _unitOfWork.AttendanceStatusesRepository.Update(attendanceStatuses);
            _unitOfWork.Save();
        }

        public async Task<AttendanceStatusesDto> GetBy(AttendanceStatusesDto attendanceStatusesDto)
        {
            Expression<Func<AttendanceStatuses, bool>> expression = (
                   l => (attendanceStatusesDto.Id == 0 || l.Id == attendanceStatusesDto.Id) &&
                       (attendanceStatusesDto.NameAr == "" || l.NameAr.Contains(attendanceStatusesDto.NameAr)
                       || attendanceStatusesDto.NameEn == "" || l.NameEn.Contains(attendanceStatusesDto.NameEn)));

            AttendanceStatuses data = await _unitOfWork.AttendanceStatusesRepository.GetBy(expression);
            AttendanceStatusesDto mapperData = _mapper.Map<AttendanceStatusesDto>(data);
            return mapperData;
        }

        public AttendanceStatusesDto GetById(int id)
        {
            AttendanceStatuses attendanceStatuses = _unitOfWork.AttendanceStatusesRepository.GetById(id);
            AttendanceStatusesDto attendanceStatusesDto = _mapper.Map<AttendanceStatusesDto>(attendanceStatuses);
            return attendanceStatusesDto;
        }

        public void Insert(AttendanceStatusesDto attendanceStatusesDto)
        {
            AttendanceStatuses attendanceStatuses = _mapper.Map<AttendanceStatuses>(attendanceStatusesDto);
            _unitOfWork.AttendanceStatusesRepository.Add(attendanceStatuses);
            _unitOfWork.Save();
        }

        public async Task<(IList<AttendanceStatusesDto> AttendanceStatuses, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.attendanceStatuses == null)
                result.attendanceStatuses = new List<AttendanceStatusesDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.attendanceStatuses, filteredCount, totalCount);
        }
        private async Task<(IList<AttendanceStatusesDto> attendanceStatuses, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<AttendanceStatuses> data = await _unitOfWork.AttendanceStatusesRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<AttendanceStatusesDto> result = _mapper.Map<List<AttendanceStatusesDto>>(data);

            int filteredResultsCount = _unitOfWork.AttendanceStatusesRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.AttendanceStatusesRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<AttendanceStatuses, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<AttendanceStatuses, bool>> expression = (a => a.IsDeleted != true );
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true  ) && (a.NameAr.Contains(searchFilter) || a.NameEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(AttendanceStatusesDto attendanceStatusesDto)
        {
            attendanceStatusesDto.ToUpdatable();
            AttendanceStatuses attendanceStatuses = _unitOfWork.AttendanceStatusesRepository.GetById(attendanceStatusesDto.Id, true);
            _mapper.Map<AttendanceStatusesDto, AttendanceStatuses>(attendanceStatusesDto, attendanceStatuses);

            _unitOfWork.AttendanceStatusesRepository.Update(attendanceStatuses);
            _unitOfWork.Save();
        }
    }
}
