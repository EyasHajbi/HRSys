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
    public class TeamsService : ITeamsService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public TeamsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<TeamsDto>> All(bool ignoreDeleted = true, Expression<Func<Teams, bool>> expression = null)
        {
            IEnumerable<Teams> data = await _unitOfWork.TeamsRepository.All(expression);
            List<TeamsDto> list = _mapper.Map<List<TeamsDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            Teams teams = _unitOfWork.TeamsRepository.GetById(Id, true);
            teams.IsDeleted = true;
            teams.ModifiedDate = DateTime.Now;
            _unitOfWork.TeamsRepository.Update(teams);
            _unitOfWork.Save();
        }

        public async Task<TeamsDto> GetBy(TeamsDto teamsDto)
        {
            Expression<Func<Teams, bool>> expression = (
                   l => (teamsDto.Id == 0 || l.Id == teamsDto.Id) &&
                       (teamsDto.NameAr == "" || l.NameAr.Contains(teamsDto.NameAr)
                       || teamsDto.NameEn == "" || l.NameEn.Contains(teamsDto.NameEn)));

            Teams data = await _unitOfWork.TeamsRepository.GetBy(expression);
            TeamsDto mapperData = _mapper.Map<TeamsDto>(data);
            return mapperData;
        }

        public TeamsDto GetById(int id)
        {
            Teams teams = _unitOfWork.TeamsRepository.GetById(id);
            TeamsDto teamsDto = _mapper.Map<TeamsDto>(teams);
            return teamsDto;
        }

        public void Insert(TeamsDto teamsDto)
        {
            Teams teams = _mapper.Map<Teams>(teamsDto);
            _unitOfWork.TeamsRepository.Add(teams);
            _unitOfWork.Save();
        }

        public async Task<(IList<TeamsDto> Teams, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.teams == null)
                result.teams = new List<TeamsDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.teams, filteredCount, totalCount);
        }
        private async Task<(IList<TeamsDto> teams, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<Teams> data = await _unitOfWork.TeamsRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<TeamsDto> result = _mapper.Map<List<TeamsDto>>(data);

            int filteredResultsCount = _unitOfWork.TeamsRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.TeamsRepository.All().Result.Where(a => a.IsDeleted != true).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<Teams, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<Teams, bool>> expression = (a => a.IsDeleted != true);
            if (!String.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.Trim();
                expression = (a => (a.IsDeleted != true ) && (a.NameAr.Contains(searchFilter) || a.NameEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(TeamsDto teamsDto)
        {
            Teams teams = _unitOfWork.TeamsRepository.GetById(teamsDto.Id, true);
            _mapper.Map<TeamsDto, Teams>(teamsDto, teams);

            _unitOfWork.TeamsRepository.Update(teams);
            _unitOfWork.Save();
        }
    }
}
