﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Users
{
    public class LoginMemberDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
