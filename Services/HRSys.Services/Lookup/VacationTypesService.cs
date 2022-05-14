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
    public class VacationTypesService : IVacationTypesService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public VacationTypesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<VacationTypesDto>> All(bool ignoreDeleted = true, Expression<Func<VacationTypes, bool>> expression = null)
        {
            IEnumerable<VacationTypes> data = await _unitOfWork.VacationTypesRepository.All(expression);
            List<VacationTypesDto> list = _mapper.Map<List<VacationTypesDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            VacationTypes vacationTypes = _unitOfWork.VacationTypesRepository.GetById(Id, true);
            vacationTypes.IsDeleted = true;
            vacationTypes.ModifiedDate = DateTime.Now;
            _unitOfWork.VacationTypesRepository.Update(vacationTypes);
            _unitOfWork.Save();
        }

        public async Task<VacationTypesDto> GetBy(VacationTypesDto vacationTypesDto)
        {
            Expression<Func<VacationTypes, bool>> expression = (
                   l => (vacationTypesDto.Id == 0 || l.Id == vacationTypesDto.Id) &&
                       (vacationTypesDto.DescriptionAr == "" || l.DescriptionAr.Contains(vacationTypesDto.DescriptionAr)
                       || vacationTypesDto.DescriptionEn == "" || l.DescriptionEn.Contains(vacationTypesDto.DescriptionEn)));

            VacationTypes data = await _unitOfWork.VacationTypesRepository.GetBy(expression);
            VacationTypesDto mapperData = _mapper.Map<VacationTypesDto>(data);
            return mapperData;
        }

        public VacationTypesDto GetById(int id)
        {
            VacationTypes vacationTypes = _unitOfWork.VacationTypesRepository.GetById(id);
            VacationTypesDto vacationTypesDto = _mapper.Map<VacationTypesDto>(vacationTypes);
            return vacationTypesDto;
        }

        public void Insert(VacationTypesDto vacationTypesDto)
        {
            VacationTypes vacationTypes = _mapper.Map<VacationTypes>(vacationTypesDto);
            _unitOfWork.VacationTypesRepository.Add(vacationTypes);
            _unitOfWork.Save();
        }

        public async Task<(IList<VacationTypesDto> VacationTypes, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.vacationTypes == null)
                result.vacationTypes = new List<VacationTypesDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.vacationTypes, filteredCount, totalCount);
        }
        private async Task<(IList<VacationTypesDto> vacationTypes, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<VacationTypes> data = await _unitOfWork.VacationTypesRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<VacationTypesDto> result = _mapper.Map<List<VacationTypesDto>>(data);

            int filteredResultsCount = _unitOfWork.VacationTypesRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.VacationTypesRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<VacationTypes, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<VacationTypes, bool>> expression = (a => a.IsDeleted != true && a.Code != "SOS");
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true && a.Code != "SOS") && (a.DescriptionAr.Contains(searchFilter) || a.DescriptionEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(VacationTypesDto vacationTypesDto)
        {
            vacationTypesDto.ToUpdatable();
            VacationTypes vacationTypes = _unitOfWork.VacationTypesRepository.GetById(vacationTypesDto.Id, true);
            _mapper.Map<VacationTypesDto, VacationTypes>(vacationTypesDto, vacationTypes);

            _unitOfWork.VacationTypesRepository.Update(vacationTypes);
            _unitOfWork.Save();
        }
    }
}
