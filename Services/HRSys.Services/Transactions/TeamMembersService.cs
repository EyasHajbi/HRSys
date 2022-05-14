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
    public class TeamMembersService : ITeamMembersService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion
        public TeamMembersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<TeamMembersDto>> All(bool ignoreDeleted = true, Expression<Func<TeamMembers, bool>> expression = null)
        {
            IEnumerable<TeamMembers> data = await _unitOfWork.TeamMembersRepository.All(expression);
            List<TeamMembersDto> list = _mapper.Map<List<TeamMembersDto>>(data);
            return list;
        }





        public void Delete(int Id)
        {
            _unitOfWork.TeamMembersRepository.Delete(Id);
            _unitOfWork.Save();
        }

        public async Task<TeamMembersDto> GetBy(TeamMembersDto teamMembersDto)
        {
            Expression<Func<TeamMembers, bool>> expression = (
                   l => (teamMembersDto.Id == 0 || l.Id == teamMembersDto.Id));

            TeamMembers data = await _unitOfWork.TeamMembersRepository.GetBy(expression);
            TeamMembersDto mapperData = _mapper.Map<TeamMembersDto>(data);
            return mapperData;
        }

        public TeamMembersDto GetById(int id)
        {
            TeamMembers teamMembers = _unitOfWork.TeamMembersRepository.GetById(id);
            TeamMembersDto teamMembersDto = _mapper.Map<TeamMembersDto>(teamMembers);
            return teamMembersDto;
        }

        public void Insert(TeamMembersDto teamMembersDto)
        {
            TeamMembers teamMembers = _mapper.Map<TeamMembers>(teamMembersDto);
            _unitOfWork.TeamMembersRepository.Add(teamMembers);
            _unitOfWork.Save();
        }

        public async Task<(IList<TeamMembersDto> TeamMembers, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, Lang CurrentLang)
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

            if (result.teamMembers == null)
                result.teamMembers = new List<TeamMembersDto>();
            else
            {
                filteredCount = result.filteredResultsCount;
                totalCount = result.totalResultsCount;
            }
            return (result.teamMembers, filteredCount, totalCount);
        }
        private async Task<(IList<TeamMembersDto> teamMembers, int filteredResultsCount, int totalResultsCount)> ListPagingExtra(string searchBy, int take, int skip, string sortBy, bool sortDir, Lang lang)
        {
            var where = BuildWhere(searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "Id";
                sortDir = true;
            }
            IEnumerable<TeamMembers> data = await _unitOfWork.TeamMembersRepository.All(where);

            data = data.OrderByDescending(a => a.Id)
                           .Skip(skip)
                           .Take(take)
                           .ToList();


            List<TeamMembersDto> result = _mapper.Map<List<TeamMembersDto>>(data);

            int filteredResultsCount = _unitOfWork.TeamMembersRepository.All(where).Result.Count();
            int totalResultsCount = _unitOfWork.TeamMembersRepository.All().Result.Where(a => a.Id >0).Count();

            return (result, filteredResultsCount, totalResultsCount);
        }
        private Expression<Func<TeamMembers, bool>> BuildWhere(string searchFilter)
        {
            Expression<Func<TeamMembers, bool>> expression = (a => a.Id >0);
            if (!String.IsNullOrEmpty(searchFilter))
            {
                //searchFilter = searchFilter.Trim();
                //expression = (a => (a.IsDeleted != true ) && (a.NameAr.Contains(searchFilter) || a.NameEn.Contains(searchFilter)));
            }
            return expression;
        }

        public void Update(TeamMembersDto teamMembersDto)
        {
            TeamMembers teamMembers = _unitOfWork.TeamMembersRepository.GetById(teamMembersDto.Id, true);
            _mapper.Map<TeamMembersDto, TeamMembers>(teamMembersDto, teamMembers);

            _unitOfWork.TeamMembersRepository.Update(teamMembers);
            _unitOfWork.Save();
        }
    }
}
