using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Common
{
   public class KeyValueDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
        public bool Selected { get; set; }
    }
}
