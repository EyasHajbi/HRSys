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
using HRSys.DTO.Transactions;

namespace HRSys.Services.Transactions
{
    public class AttendanceTransactionsService : IAttendanceTransactionsService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public AttendanceTransactionsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<AttendanceTransactionsDto>> All(bool ignoreDeleted = true, Expression<Func<AttendanceTransactions, bool>> expression = null)
        {
            IEnumerable<AttendanceTransactions> data = await _unitOfWork.AttendanceTransactionsRepository.All(expression);
            List<AttendanceTransactionsDto> list = _mapper.Map<List<AttendanceTransactionsDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            AttendanceTransactions attendanceTransactions = _unitOfWork.AttendanceTransactionsRepository.GetById(Id, true);
            attendanceTransactions.IsDeleted = true;
            attendanceTransactions.ModifiedDate = DateTime.Now;
            _unitOfWork.AttendanceTransactionsRepository.Update(attendanceTransactions);
            _unitOfWork.Save();
        }

        public async Task<AttendanceTransactionsDto> GetBy(AttendanceTransactionsDto attendanceTransactionsDto)
        {
            Expression<Func<AttendanceTransactions, bool>> expression = (
                   l => (attendanceTransactionsDto.Id == 0 || l.Id == attendanceTransactionsDto.Id) &&
                       (attendanceTransactionsDto.EmployeeName == "" || l.EmployeeName.Contains(attendanceTransactionsDto.EmployeeName)));

            AttendanceTransactions data = await _unitOfWork.AttendanceTransactionsRepository.GetBy(expression);
            AttendanceTransactionsDto mapperData = _mapper.Map<AttendanceTransactionsDto>(data);
            return mapperData;
        }

        public AttendanceTransactionsDto GetById(int id)
        {
            AttendanceTransactions attendanceTransactions = _unitOfWork.AttendanceTransactionsRepository.GetById(id);
            AttendanceTransactionsDto attendanceTransactionsDto = _mapper.Map<AttendanceTransactionsDto>(attendanceTransactions);
            return attendanceTransactionsDto;
        }

        public void Insert(AttendanceTransactionsDto attendanceTransactionsDto)
        {
            AttendanceTransactions attendanceTransactions = _mapper.Map<AttendanceTransactions>(attendanceTransactionsDto);
            _unitOfWork.AttendanceTransactionsRepository.Add(attendanceTransactions);
            _unitOfWork.Save();
        }

        public async Task<(IList<AttendanceTransactionsDto> AttendanceTransactions, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.attendanceTransactions == null)
                result.attendanceTransactions = new List<AttendanceTransactionsDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.attendanceTransactions, filteredCount, totalCount);
        }
        private async Task<(IList<AttendanceTransactionsDto> attendanceTransactions, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<AttendanceTransactions> data = await _unitOfWork.AttendanceTransactionsRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<AttendanceTransactionsDto> result = _mapper.Map<List<AttendanceTransactionsDto>>(data);

            int filteredResultsCount = _unitOfWork.AttendanceTransactionsRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.AttendanceTransactionsRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<AttendanceTransactions, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<AttendanceTransactions, bool>> expression = (a => a.IsDeleted != true);
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true ) && (a.EmployeeName.Contains(searchFilter) || a.EmployeeName.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(AttendanceTransactionsDto attendanceTransactionsDto)
        {
            AttendanceTransactions attendanceTransactions = _unitOfWork.AttendanceTransactionsRepository.GetById(attendanceTransactionsDto.Id, true);
            _mapper.Map<AttendanceTransactionsDto, AttendanceTransactions>(attendanceTransactionsDto, attendanceTransactions);

            _unitOfWork.AttendanceTransactionsRepository.Update(attendanceTransactions);
            _unitOfWork.Save();
        }
    }
}
