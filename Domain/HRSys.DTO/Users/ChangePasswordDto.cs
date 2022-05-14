using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Users
{
   public class ChangePasswordDto
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
