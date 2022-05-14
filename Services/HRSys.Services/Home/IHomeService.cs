using HRSys.DTO.Common;
using HRSys.DTO.Home;
using HRSys.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Home
{
  public   interface IHomeService
    {
        //Task<(IList<HomeResultDTO> HomeResultDTOs, int filteredResultsCount, int totalResultsCount)> ListPaging(DataTableUiDto model, string currentUserId, int complaintsType, int? complaintsTypeId, DateTime? complaintDateFr, DateTime? complaintDateTo, Lang CurrentLang);
    }
}
