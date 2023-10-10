using SAMS.Infrastructure.Constants;
using SAMS.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SAMS.Infrastructure.WebToken
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public JwtHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetValueFromToken(string propertyName)
        {
            try
            {
                var jwt = httpContextAccessor.HttpContext.Request.Headers["Authorization"];

                var handler = new JwtSecurityTokenHandler();
                var tokens = handler.ReadToken(jwt[0].Replace("Bearer ", "")) as JwtSecurityToken;
                return tokens.Claims.FirstOrDefault(claim => claim.Type == propertyName).Value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public List<string> GetValueListFromToken(string propertyName)
        {
            try
            {
                var jwt = httpContextAccessor.HttpContext.Request.Headers["Authorization"];

                var handler = new JwtSecurityTokenHandler();
                var tokens = handler.ReadToken(jwt[0].Replace("Bearer ", "")) as JwtSecurityToken;

                var claims = tokens.Claims.Where(claim => claim.Type == propertyName).ToList();

                var valueList = new List<string>();
                foreach (var claim in claims)
                {
                    valueList.Add(claim.Value);
                }
                return valueList;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public bool IsInRole(string role, bool? findAllRoles = false)
        {
            if (findAllRoles.Value)
            {
                var stringRoles = GetValueListFromToken("roles");
                var roles = new List<UserInfoRoleDto>();
                foreach (var stringRole in stringRoles)
                {
                    roles.Add(JsonConvert.DeserializeObject<UserInfoRoleDto>(stringRole));
                }

                return roles.Any(d=> d.RoleName == role);
            }
            else
            {
                var stringSelectedRole = GetValueFromToken("selectedRole");
                var selectedRole = JsonConvert.DeserializeObject<UserInfoRoleDto>(stringSelectedRole);

                return selectedRole.RoleName == role;
            }
        }

        public UserInfoDto GetCurrentUser()
        {
            UserInfoDto userInfo = new UserInfoDto();
            userInfo.Id = long.Parse(GetValueFromToken("userId"));
            userInfo.Name = GetValueFromToken("name");
            userInfo.Surname = GetValueFromToken("surname");
            userInfo.Email = GetValueFromToken("email");
            userInfo.UserIp = GetValueFromToken("userIp");
            userInfo.Phone = GetValueFromToken("phone");
            userInfo.ImageUrl = GetValueFromToken("imageUrl");
            var roles = GetValueListFromToken("roles");
            userInfo.Roles = new List<UserInfoRoleDto>();
            foreach (var role in roles)
            {
                userInfo.Roles.Add(JsonConvert.DeserializeObject<UserInfoRoleDto>(role));
            }
            userInfo.SelectedRole = JsonConvert.DeserializeObject<UserInfoRoleDto>(GetValueFromToken("selectedRole"));

            return userInfo;
        }

        public UserInfoDto GenerateJwtToken(UserInfoDto userInfo, UserInfoRoleDto? selectedRole = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Keys.JwtKey);

            var claims = new List<Claim>
            {
                new Claim("userId", userInfo.Id.ToString()),
                new Claim("name", userInfo.Name),
                new Claim("surname", userInfo.Surname),
                new Claim("email", userInfo.Email),
                new Claim("phone", userInfo.Phone ?? ""),
                new Claim("userIp", userInfo.UserIp),
                new Claim("imageUrl", userInfo.ImageUrl ?? "")
            };

            foreach (var role in userInfo.Roles)
            {
                claims.Add(new Claim("roles", JsonConvert.SerializeObject(role)));
            }
            if (selectedRole != null)
            {
                userInfo.SelectedRole = selectedRole;
                claims.Add(new Claim("selectedRole", JsonConvert.SerializeObject(userInfo.SelectedRole)));
            }
            else if (userInfo.Roles.Count == 1)
            {
                claims.Add(new Claim("selectedRole", JsonConvert.SerializeObject(userInfo.Roles[0])));
                userInfo.SelectedRole = userInfo.Roles[0];
            }
            else
            {
                claims.Add(new Claim("selectedRole", ""));
                userInfo.SelectedRole = null;
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            userInfo.Token = tokenHandler.WriteToken(securityToken);

            //HttpContext.Request.Headers["Authorization"];

            return userInfo;
        }
    }
}
