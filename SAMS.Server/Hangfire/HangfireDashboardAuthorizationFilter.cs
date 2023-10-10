using SAMS.DataAccess;
using SAMS.Infrastructure.WebToken;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Newtonsoft.Json;
using SAMS.Infrastructure.Models;

namespace SAMS.Server.Hangfire
{
    public class HangfireDashboardJwtAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private static readonly string HangFireCookieName = "HangFireCookie";
        private static readonly int CookieExpirationMinutes = 60;
        private string role;

        public HangfireDashboardJwtAuthorizationFilter(string role = null)
        {
            this.role = role;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var access_token = string.Empty;
            var setCookie = false;

            // try to get token from query string
            if (httpContext.Request.Query.ContainsKey("access_token"))
            {
                access_token = httpContext.Request.Query["access_token"].FirstOrDefault();
                setCookie = true;
            }
            else
            {
                access_token = httpContext.Request.Cookies[HangFireCookieName];
            }

            if (string.IsNullOrEmpty(access_token))
            {
                return false;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var tokens = handler.ReadToken(access_token) as JwtSecurityToken;
                var str_tokenRole = tokens.Claims.FirstOrDefault(claim => claim.Type == "selectedRole").Value;
                UserInfoRoleDto selectedRole = JsonConvert.DeserializeObject<UserInfoRoleDto>(str_tokenRole);
                
                if (role != selectedRole.RoleName)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            if (setCookie)
            {
                httpContext.Response.Cookies.Append(HangFireCookieName,
                access_token,
                new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(CookieExpirationMinutes)
                });
            }


            return true;
        }
    }
}
