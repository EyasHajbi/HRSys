using HRSys.Model;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HRSys.Repositories.Generic.Interface;
using HRSys.Enum;
using HRSys.Repositories.Generic;
using HRSys.Repositories.Transactions.Interface;

namespace HRSys.Transactions
{
    public class TasksRepository : GenericRepository<HRSys.Model.Tasks>, ITasksRepository
    {
        public TasksRepository(HRSysContext context) : base(context)
        {
        }
    }
}
