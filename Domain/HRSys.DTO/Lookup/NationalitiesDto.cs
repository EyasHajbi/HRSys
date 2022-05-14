using HRSys.DTO.App_Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Lookup
{
	public class NationalitiesDto : IUpdatableDto
	{
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; } 
        public bool IsDeleted { get; set; }
        public bool IsUpdateOperation { get; set; }
    }
}
