using AutoMapper;
using HRSys.DTO.Common;
using HRSys.DTO.Lookup;
using HRSys.Enum;
using HRSys.Model;
using HRSys.Repositories.Generic.Interface;
using HRSys.Services.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Lookup
{
    public class NotificationsService : INotificationsService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public NotificationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<NotificationsDto>> All(bool ignoreDeleted = true, Expression<Func<Notifications, bool>> expression = null)
        {
            IEnumerable<Notifications> data = await _unitOfWork.NotificationsRepository.All(expression);
            List<NotificationsDto> list = _mapper.Map<List<NotificationsDto>>(data);
            return list;
        }

       
        public void Delete(int Id)
        {
            Notifications notifications = _unitOfWork.NotificationsRepository.GetById(Id, true);
            notifications.IsDeleted = true;
            notifications.ModifiedDate = DateTime.Now;
            _unitOfWork.NotificationsRepository.Update(notifications);
            _unitOfWork.Save();
        }

        public async Task<NotificationsDto> GetBy(NotificationsDto notificationsDto)
        {
            Expression<Func<Notifications, bool>> expression = (
                   l => (notificationsDto.Id == 0 || l.Id == notificationsDto.Id) &&
                       (notificationsDto.DescriptionAr == "" || l.DescriptionAr.Contains(notificationsDto.DescriptionAr)
                       || notificationsDto.DescriptionEn == "" || l.DescriptionEn.Contains(notificationsDto.DescriptionEn)));

            Notifications data = await _unitOfWork.NotificationsRepository.GetBy(expression);
            NotificationsDto mapperData = _mapper.Map<NotificationsDto>(data);
            return mapperData;
        }

        public NotificationsDto GetById(int id)
        {
            Notifications notifications = _unitOfWork.NotificationsRepository.GetById(id);
            NotificationsDto notificationsDto = _mapper.Map<NotificationsDto>(notifications);
            return notificationsDto;
        }

        public void Insert(NotificationsDto notificationsDto)
        {
            Notifications notifications = _mapper.Map<Notifications>(notificationsDto);
            _unitOfWork.NotificationsRepository.Add(notifications);
            _unitOfWork.Save();
        }

        public async Task<(IList<NotificationsDto> Notifications, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.notifications == null)
                result.notifications = new List<NotificationsDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.notifications, filteredCount, totalCount);
        }
        private async Task<(IList<NotificationsDto> notifications, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<Notifications> data = await _unitOfWork.NotificationsRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<NotificationsDto> result = _mapper.Map<List<NotificationsDto>>(data);

            int filteredResultsCount = _unitOfWork.NotificationsRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.NotificationsRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<Notifications, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<Notifications, bool>> expression = (a => a.IsDeleted != true);
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true) && (a.DescriptionAr.Contains(searchFilter) || a.DescriptionEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(NotificationsDto notificationsDto)
        {
            notificationsDto.ToUpdatable();
            Notifications notifications = _unitOfWork.NotificationsRepository.GetById(notificationsDto.Id,true);
            notificationsDto.DateOfStatus = notifications.DateOfStatus;
            _mapper.Map<NotificationsDto, Notifications>(notificationsDto, notifications);
           
            _unitOfWork.NotificationsRepository.Update(notifications);
            _unitOfWork.Save();
        }
    }
}
