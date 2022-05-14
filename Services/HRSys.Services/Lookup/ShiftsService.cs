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
    public class ShiftsService : IShiftsService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public ShiftsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<ShiftsDto>> All(bool ignoreDeleted = true, Expression<Func<Shifts, bool>> expression = null)
        {
            IEnumerable<Shifts> data = await _unitOfWork.ShiftsRepository.All(expression);
            List<ShiftsDto> list = _mapper.Map<List<ShiftsDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            Shifts shifts = _unitOfWork.ShiftsRepository.GetById(Id, true);
            shifts.IsDeleted = true;
            shifts.ModifiedDate = DateTime.Now;
            _unitOfWork.ShiftsRepository.Update(shifts);
            _unitOfWork.Save();
        }

        public async Task<ShiftsDto> GetBy(ShiftsDto shiftsDto)
        {
            Expression<Func<Shifts, bool>> expression = (
                   l => (shiftsDto.Id == 0 || l.Id == shiftsDto.Id) &&
                       (shiftsDto.DescriptionAr == "" || l.DescriptionAr.Contains(shiftsDto.DescriptionAr)
                       || shiftsDto.DescriptionEn == "" || l.DescriptionEn.Contains(shiftsDto.DescriptionEn)));

            Shifts data = await _unitOfWork.ShiftsRepository.GetBy(expression);
            ShiftsDto mapperData = _mapper.Map<ShiftsDto>(data);
            return mapperData;
        }

        public ShiftsDto GetById(int id)
        {
            Shifts shifts = _unitOfWork.ShiftsRepository.GetById(id);
            ShiftsDto shiftsDto = _mapper.Map<ShiftsDto>(shifts);
            return shiftsDto;
        }

        public void Insert(ShiftsDto shiftsDto)
        {
            Shifts shifts = _mapper.Map<Shifts>(shiftsDto);
            _unitOfWork.ShiftsRepository.Add(shifts);
            _unitOfWork.Save();
        }

        public async Task<(IList<ShiftsDto> Shifts, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.shifts == null)
                result.shifts = new List<ShiftsDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.shifts, filteredCount, totalCount);
        }
        private async Task<(IList<ShiftsDto> shifts, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<Shifts> data = await _unitOfWork.ShiftsRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<ShiftsDto> result = _mapper.Map<List<ShiftsDto>>(data);

            int filteredResultsCount = _unitOfWork.ShiftsRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.ShiftsRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<Shifts, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<Shifts, bool>> expression = (a => a.IsDeleted != true && a.Code != "SOS");
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true && a.Code != "SOS") && (a.DescriptionAr.Contains(searchFilter) || a.DescriptionEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(ShiftsDto shiftsDto)
        {
            shiftsDto.ToUpdatable();
            Shifts shifts = _unitOfWork.ShiftsRepository.GetById(shiftsDto.Id, true);
            _mapper.Map<ShiftsDto, Shifts>(shiftsDto, shifts);

            _unitOfWork.ShiftsRepository.Update(shifts);
            _unitOfWork.Save();
        }
    }
}
