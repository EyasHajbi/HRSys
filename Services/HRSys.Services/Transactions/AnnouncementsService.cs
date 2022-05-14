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
    public class AnnouncementsService : IAnnouncementsService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public AnnouncementsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<AnnouncementsDto>> All(bool ignoreDeleted = true, Expression<Func<Announcements, bool>> expression = null)
        {
            IEnumerable<Announcements> data = await _unitOfWork.AnnouncementsRepository.All(expression);
            List<AnnouncementsDto> list = _mapper.Map<List<AnnouncementsDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            Announcements announcements = _unitOfWork.AnnouncementsRepository.GetById(Id, true);
            announcements.IsDeleted = true;
            announcements.ModifiedDate = DateTime.Now;
            _unitOfWork.AnnouncementsRepository.Update(announcements);
            _unitOfWork.Save();
        }

        public async Task<AnnouncementsDto> GetBy(AnnouncementsDto announcementsDto)
        {
            Expression<Func<Announcements, bool>> expression = (
                   l => (announcementsDto.Id == 0 || l.Id == announcementsDto.Id) &&
                       (announcementsDto.DescriptionAr == "" || l.DescriptionAr.Contains(announcementsDto.DescriptionAr)
                       || announcementsDto.DescriptionEn == "" || l.DescriptionEn.Contains(announcementsDto.DescriptionEn)));

            Announcements data = await _unitOfWork.AnnouncementsRepository.GetBy(expression);
            AnnouncementsDto mapperData = _mapper.Map<AnnouncementsDto>(data);
            return mapperData;
        }

        public AnnouncementsDto GetById(int id)
        {
            Announcements announcements = _unitOfWork.AnnouncementsRepository.GetById(id);
            AnnouncementsDto announcementsDto = _mapper.Map<AnnouncementsDto>(announcements);
            return announcementsDto;
        }

        public void Insert(AnnouncementsDto announcementsDto)
        {
            Announcements announcements = _mapper.Map<Announcements>(announcementsDto);
            _unitOfWork.AnnouncementsRepository.Add(announcements);
            _unitOfWork.Save();
        }

        public async Task<(IList<AnnouncementsDto> Announcements, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.announcements == null)
                result.announcements = new List<AnnouncementsDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.announcements, filteredCount, totalCount);
        }
        private async Task<(IList<AnnouncementsDto> announcements, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<Announcements> data = await _unitOfWork.AnnouncementsRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<AnnouncementsDto> result = _mapper.Map<List<AnnouncementsDto>>(data);

            int filteredResultsCount = _unitOfWork.AnnouncementsRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.AnnouncementsRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<Announcements, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<Announcements, bool>> expression = (a => a.IsDeleted != true);
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true ) && (a.DescriptionAr.Contains(searchFilter) || a.DescriptionEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(AnnouncementsDto announcementsDto)
        {
            Announcements announcements = _unitOfWork.AnnouncementsRepository.GetById(announcementsDto.Id, true);
            _mapper.Map<AnnouncementsDto, Announcements>(announcementsDto, announcements);

            _unitOfWork.AnnouncementsRepository.Update(announcements);
            _unitOfWork.Save();
        }
    }
}
