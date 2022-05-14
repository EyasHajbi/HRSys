using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRSys.Api.Helpers
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUserInfo CurrentUserInfo()
        {
            return JsonConvert.DeserializeObject<CurrentUserInfo>(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData).Value);
        }
        public string UserId
        {
            get
            {
                return CurrentUserInfo().UserId;
            }
        }
        public int CompanyId
        {
            get
            {
                return CurrentUserInfo().CompanyId;
            }
        }

        public bool IsRightToLeft
        {
            get
            {
                var feature = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
                return feature.RequestCulture.Culture.TextInfo.IsRightToLeft;

            }
        }

        public string LastName
        {
            get
            {
                if (CurrentUserInfo() != null)
                    return CurrentUserInfo().LastName;
                else
                    return "";
            }
        }
        public string FirstName
        {
            get
            {
                if (CurrentUserInfo() != null)
                    return CurrentUserInfo().FirstName;
                else
                    return "";
            }
        }

    }
}
