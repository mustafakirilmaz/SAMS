using SAMS.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SAMS.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RolesAttribute : Attribute, IAuthorizationFilter
    {
        private string[] _roles;
        public RolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var auth = context.HttpContext.Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(auth[0].Replace("Bearer ", "")) as JwtSecurityToken;
            if (token == null)
            {
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }

            var str_roles = token.Claims.Where(claim => claim.Type == "roles").Select(d => d.Value).ToList();
            var str_selectedRole = token.Claims.Where(claim => claim.Type == "selectedRole").Select(d => d.Value).FirstOrDefault();

            UserInfoRoleDto selectedRole = JsonConvert.DeserializeObject<UserInfoRoleDto>(str_selectedRole);
            List<UserInfoRoleDto> roles = new List<UserInfoRoleDto>();
            foreach (var str_role in str_roles)
            {
                roles.Add(JsonConvert.DeserializeObject<UserInfoRoleDto>(str_role));
            }
            if (!roles.Any(d=> d.Id == selectedRole.Id) || !_roles.Any(d=> d == selectedRole.RoleName))
            {
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            }
        }
    }
}
