﻿using System;
using System.Collections.Generic;

namespace HRSys.DTO.Lookup
{
    public partial class TasksStatusDto : IUpdatableDto
    {

        public TasksStatusDto()
        {
        }
        public int Id { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSystematic { get; set; }
        public bool IsUpdateOperation { get; set; }
    }
}
