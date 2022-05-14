using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.Model
{
    public class ApplicationSettings
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }
        public bool ClearCache { get; set; }
    }
}
