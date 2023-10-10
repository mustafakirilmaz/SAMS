using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Infrastructure.WebToken
{
    public interface IJwtHelper
    {
        string GetValueFromToken(string propertyName);
        List<string> GetValueListFromToken(string propertyName);
        bool IsInRole(string role, bool? findAllRoles = false);
        UserInfoDto GetCurrentUser();
        UserInfoDto GenerateJwtToken(UserInfoDto jwtTokenDto, UserInfoRoleDto? selectedRole = null);
    }
}
