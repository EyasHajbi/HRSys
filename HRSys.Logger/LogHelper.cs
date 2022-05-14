using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HRSys.Logger
{
    public class LogHelper
    {
        public static void LogError(Exception ex, ClaimsPrincipal user, Dictionary<string, object> additionalInfo = null)
        {
            var detailsToWrite = GetLogDetail(user, additionalInfo);
            detailsToWrite.Exception = ex;

            HRSys.Logger.Logger.Error(detailsToWrite);
        }

        private static LogItem GetLogDetail(ClaimsPrincipal user, Dictionary<string, object> additionalInfo = null)
        {
            var details = new LogItem();

            GetRequestData(details, additionalInfo);
            GetUserData(details, user);

            return details;
        }

        private static void GetRequestData(LogItem detail, Dictionary<string, object> additionalInfo = null)
        {
            if (additionalInfo != null)
            {
                foreach (var key in additionalInfo.Keys)
                    detail.AdditionalInfo.Add($"QueryString-{key}", additionalInfo[key]);
            }
        }

        private static void GetUserData(LogItem detail, ClaimsPrincipal user)
        {
            var userId = "N/A";
            var userName = "N/A";
            if (user != null)
            {
                var i = 1;
                foreach (var claim in user.Claims)
                {
                    if (claim.Value == "Id")
                        userId = claim.Value;
                    else if (claim.Type == "Name")
                        userName = claim.Value;
                    else
                        detail.AdditionalInfo.Add($"UserClaim-{i++}-{claim.Type}", claim.Value);
                }
            }
            detail.UserId = userId;
            detail.UserName = userName;
        }
    }
}
