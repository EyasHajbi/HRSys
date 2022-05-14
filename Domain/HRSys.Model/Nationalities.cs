using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class Nationalities
    {
        public Nationalities()
        {
        }
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
