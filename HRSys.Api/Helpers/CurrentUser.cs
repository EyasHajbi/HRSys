using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HRSys.Api.Helpers
{
    public static class CurrentUser
    {
        public static string GetUserId(this IPrincipal principal, string token = "")
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var jti = jwtSecurityToken.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value;
                ////var CurrentUserInfo = JsonConvert.DeserializeObject<CurrentUserInfo>(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
                ////return CurrentUserInfo.UserId;
                //var claimsIdentity = (ClaimsIdentity)principal.Identity;
                //var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                //return claim.Value;
                return jti;
            }
            catch
            {
                return "";
            }
           
        }

        //public static ApplicationUser GetUser(this IPrincipal principal)
        //{
        //    ApplicationUser _user = new ApplicationUser();
        //    if (principal.Identity.IsAuthenticated)
        //    {
        //        var claimsIndentity = principal.Identity as ClaimsIdentity;
        //        var userClaims = claimsIndentity.Claims;

        //        if (claimsIndentity.IsAuthenticated)
        //        {
        //            foreach (var claim in userClaims)
        //            {
        //                var cType = claim.Type;
        //                var cValue = claim.Value;
        //                switch (cType)
        //                {
        //                    case ClaimTypes.NameIdentifier:
        //                        _user.Id = cValue;
        //                        break;
        //                    case ClaimTypes.Name:
        //                        _user.FullName = cValue;
        //                        break;
        //                    case ClaimTypes.MobilePhone:
        //                        _user.Mobile = cValue;
        //                        break;
        //                    case ClaimTypes.Email:
        //                        _user.Email = cValue;
        //                        break;
        //                    case ClaimTypes.DateOfBirth:
        //                        _user.BirthDate = Convert.ToDateTime(cValue);
        //                        break;
        //                    case ClaimTypes.Gender:
        //                        switch (cValue)
        //                        {
        //                            case "Male":
        //                                _user.Gender = Genders.Male;
        //                                break;
        //                            case "Female":
        //                                _user.Gender = Genders.Female;
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                        break;

        //                }
        //            }

        //        }
        //    }
        //    return _user;
        //}
    }
}
