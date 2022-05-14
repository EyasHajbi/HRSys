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
    public class NationalitiesService : INationalitiesService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public NationalitiesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<NationalitiesDto>> All(bool ignoreDeleted = true, Expression<Func<Nationalities, bool>> expression = null)
        {
            IEnumerable<Nationalities> data = await _unitOfWork.NationalitiesRepository.All(expression);
            List<NationalitiesDto> list = _mapper.Map<List<NationalitiesDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            Nationalities nationalities = _unitOfWork.NationalitiesRepository.GetById(Id, true);
            nationalities.IsDeleted = true;
            _unitOfWork.NationalitiesRepository.Update(nationalities);
            _unitOfWork.Save();
        }

        public async Task<NationalitiesDto> GetBy(NationalitiesDto nationalitiesDto)
        {
            Expression<Func<Nationalities, bool>> expression = (
                   l => (nationalitiesDto.Id == 0 || l.Id == nationalitiesDto.Id) &&
                       (nationalitiesDto.NameAr == "" || l.NameAr.Contains(nationalitiesDto.NameAr)
                       || nationalitiesDto.NameEn == "" || l.NameEn.Contains(nationalitiesDto.NameEn)));

            Nationalities data = await _unitOfWork.NationalitiesRepository.GetBy(expression);
            NationalitiesDto mapperData = _mapper.Map<NationalitiesDto>(data);
            return mapperData;
        }

        public NationalitiesDto GetById(int id)
        {
            Nationalities nationalities = _unitOfWork.NationalitiesRepository.GetById(id);
            NationalitiesDto nationalitiesDto = _mapper.Map<NationalitiesDto>(nationalities);
            return nationalitiesDto;
        }

        public void Insert(NationalitiesDto nationalitiesDto)
        {
            Nationalities nationalities = _mapper.Map<Nationalities>(nationalitiesDto);
            _unitOfWork.NationalitiesRepository.Add(nationalities);
            _unitOfWork.Save();
        }

        public async Task<(IList<NationalitiesDto> Nationalities, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.nationalities == null)
                result.nationalities = new List<NationalitiesDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.nationalities, filteredCount, totalCount);
        }
        private async Task<(IList<NationalitiesDto> nationalities, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<Nationalities> data = await _unitOfWork.NationalitiesRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<NationalitiesDto> result = _mapper.Map<List<NationalitiesDto>>(data);

            int filteredResultsCount = _unitOfWork.NationalitiesRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.NationalitiesRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<Nationalities, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<Nationalities, bool>> expression = (a => a.IsDeleted != true );
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true  ) && (a.NameAr.Contains(searchFilter) || a.NameEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(NationalitiesDto nationalitiesDto)
        {
            nationalitiesDto.ToUpdatable();
            Nationalities nationalities = _unitOfWork.NationalitiesRepository.GetById(nationalitiesDto.Id, true);
            _mapper.Map<NationalitiesDto, Nationalities>(nationalitiesDto, nationalities);

            _unitOfWork.NationalitiesRepository.Update(nationalities);
            _unitOfWork.Save();
        }
    }
}
