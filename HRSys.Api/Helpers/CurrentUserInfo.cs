using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRSys.Api.Helpers
{
    public class CurrentUserInfo
    {
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }

        IHttpContextAccessor ContextAccessor
        {
            get
            {
                return (IHttpContextAccessor)System.Web.HttpContext.Current.RequestServices.GetService(typeof(IHttpContextAccessor));
            }
        }
        public int PageSize
        {
            get
            {
                return 10;
            }

        }

        public async Task RefreshClaims(bool update = false, string userName = "")
        {


            List<Claim> claimsToAdd = new List<Claim>();
            CurrentUserInfo currentUser = new CurrentUserInfo()
            {
                UserId = this.UserId,
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                CompanyId = this.CompanyId,
                Language = this.Language,
                Password = this.Password
            };

            //List<Claim> claimsToAdd = new List<Claim>();
            //var claimsIndentity = HttpContext.User.Identity as ClaimsIdentity;
            //claimsToAdd.Add(new Claim("CurrentUser", JsonConvert.SerializeObject(currentUser)));
            //var clo = new ClaimsIdentity(claimsToAdd, ClaimTypes.UserData);
            //HttpContext.User.AddIdentity(clo);




        }

    }
}
