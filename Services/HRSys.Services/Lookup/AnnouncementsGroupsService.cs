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
    public class AnnouncementsGroupsService : IAnnouncementsGroupsService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public AnnouncementsGroupsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<AnnouncementsGroupsDto>> All(bool ignoreDeleted = true, Expression<Func<AnnouncementsGroups, bool>> expression = null)
        {
            IEnumerable<AnnouncementsGroups> data = await _unitOfWork.AnnouncementsGroupsRepository.All(expression);
            List<AnnouncementsGroupsDto> list = _mapper.Map<List<AnnouncementsGroupsDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            AnnouncementsGroups announcementsGroups = _unitOfWork.AnnouncementsGroupsRepository.GetById(Id, true);
            announcementsGroups.IsDeleted = true;
            announcementsGroups.ModifiedDate = DateTime.Now;
            _unitOfWork.AnnouncementsGroupsRepository.Update(announcementsGroups);
            _unitOfWork.Save();
        }

        public async Task<AnnouncementsGroupsDto> GetBy(AnnouncementsGroupsDto announcementsGroupsDto)
        {
            Expression<Func<AnnouncementsGroups, bool>> expression = (
                   l => (announcementsGroupsDto.Id == 0 || l.Id == announcementsGroupsDto.Id) &&
                       (announcementsGroupsDto.DescriptionAr == "" || l.DescriptionAr.Contains(announcementsGroupsDto.DescriptionAr)
                       || announcementsGroupsDto.DescriptionEn == "" || l.DescriptionEn.Contains(announcementsGroupsDto.DescriptionEn)));

            AnnouncementsGroups data = await _unitOfWork.AnnouncementsGroupsRepository.GetBy(expression);
            AnnouncementsGroupsDto mapperData = _mapper.Map<AnnouncementsGroupsDto>(data);
            return mapperData;
        }

        public AnnouncementsGroupsDto GetById(int id)
        {
            AnnouncementsGroups announcementsGroups = _unitOfWork.AnnouncementsGroupsRepository.GetById(id);
            AnnouncementsGroupsDto announcementsGroupsDto = _mapper.Map<AnnouncementsGroupsDto>(announcementsGroups);
            return announcementsGroupsDto;
        }

        public void Insert(AnnouncementsGroupsDto announcementsGroupsDto)
        {
            AnnouncementsGroups announcementsGroups = _mapper.Map<AnnouncementsGroups>(announcementsGroupsDto);
            _unitOfWork.AnnouncementsGroupsRepository.Add(announcementsGroups);
            _unitOfWork.Save();
        }

        public async Task<(IList<AnnouncementsGroupsDto> AnnouncementsGroups, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.announcementsGroups == null)
                result.announcementsGroups = new List<AnnouncementsGroupsDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.announcementsGroups, filteredCount, totalCount);
        }
        private async Task<(IList<AnnouncementsGroupsDto> announcementsGroups, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<AnnouncementsGroups> data = await _unitOfWork.AnnouncementsGroupsRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<AnnouncementsGroupsDto> result = _mapper.Map<List<AnnouncementsGroupsDto>>(data);

            int filteredResultsCount = _unitOfWork.AnnouncementsGroupsRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.AnnouncementsGroupsRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<AnnouncementsGroups, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<AnnouncementsGroups, bool>> expression = (a => a.IsDeleted != true && a.Code != "SOS");
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true && a.Code != "SOS") && (a.DescriptionAr.Contains(searchFilter) || a.DescriptionEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(AnnouncementsGroupsDto announcementsGroupsDto)
        {
            announcementsGroupsDto.ToUpdatable();
            AnnouncementsGroups announcementsGroups = _unitOfWork.AnnouncementsGroupsRepository.GetById(announcementsGroupsDto.Id, true);
            _mapper.Map<AnnouncementsGroupsDto, AnnouncementsGroups>(announcementsGroupsDto, announcementsGroups);

            _unitOfWork.AnnouncementsGroupsRepository.Update(announcementsGroups);
            _unitOfWork.Save();
        }
    }
}
