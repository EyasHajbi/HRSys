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
    public class VacationsTransactionsRepository : GenericRepository<HRSys.Model.VacationsTransactions>, IVacationsTransactionsRepository
    {

        public VacationsTransactionsRepository(HRSysContext context) : base(context)
        {

        }


        //public async Task<(IList<(Model.Complaints complaint, bool HasEditPermission)> complaints, int filteredResultsCount, int totalResultsCount)> GetComplaints(string searchBy, int take, int skip, string sortBy, bool sortDir, string currentUserId, bool hasPermission, int complaintType, int? complaintsTypeId, DateTime? complaintDateFr, DateTime? complaintDateTo)
        //{
        //    try
        //    {
        //        var allComplaints = context.Complaints.Include(a => a.App_Users).Include(x => x.ComplaintType).AsQueryable();

        //        if (complaintType == (int)ComplaintStatusEnum.New)
        //        {
        //            allComplaints = allComplaints.Where(x => x.IsDeleted != true && x.IsOpened != true);

        //        }else if (complaintType == (int)ComplaintStatusEnum.All)
        //        {
        //            allComplaints = allComplaints.Where(x => x.IsDeleted != true  );
        //        }
        //        else if (complaintType == (int)ComplaintStatusEnum.Opened)
        //        {
        //            allComplaints = allComplaints.Where(x => x.IsDeleted != true && x.IsOpened == true);
        //        }
        //        else if (complaintType == (int)ComplaintStatusEnum.Reported)
        //        {
        //            allComplaints = allComplaints.Where(x => x.IsDeleted != true && x.IsReported == true);
        //        }

        //        var complaintsData = allComplaints;


        //        if(complaintsTypeId !=null)
        //        {
        //            complaintsData = complaintsData.Where(x => x.ComplaintTypeID == complaintsTypeId);

        //        }

        //        if(complaintDateFr !=null)
        //        {
        //            complaintsData = complaintsData.Where(x => x.CreatedOn >=complaintDateFr);
        //        }

        //        if (complaintDateTo != null)
        //        {
        //            complaintsData = complaintsData.Where(x => x.CreatedOn <= complaintDateTo);
        //        }

        //        int filteredResultsCount = complaintsData.Count();
        //        int totalResultsCount = complaintsData.Count();

        //        complaintsData = complaintsData.OrderByDescending(a => a.CreatedOn)
        //                .Skip(skip)
        //                .Take(take);


        //        var data = complaintsData.ToList().Select(a =>
        //        {
        //            (Model.Complaints complaints, bool HasEditPermission) q = (a, true);
        //            return q;
        //        }
        //        ).ToList();

        //        return (data, filteredResultsCount, totalResultsCount);
        //    }

        //    catch (Exception ex)
        //    {
        //        return (null, 0, 0);
        //    }
        //}

        //public async Task<int> GetCountOfNewComplaints()
        //{
        //    int _count = 0;
        //    try
        //    {
        //        _count =  context.Complaints.Where(x => x.IsDeleted != true && x.IsOpened != true).Count();
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return _count;
            
        //}

    }
}
