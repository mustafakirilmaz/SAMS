using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAMS.Infrastructure.Attributes
{
    public class IpAddressCheckRequirement : IAuthorizationRequirement
    {
        public bool IpClaimRequired { get; set; } = true;
    }

    public class IpAddressCheckHandler : AuthorizationHandler<IpAddressCheckRequirement>
    {
        public IpAddressCheckHandler(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private IHttpContextAccessor HttpContextAccessor { get; }
        private HttpContext HttpContext => HttpContextAccessor.HttpContext;


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IpAddressCheckRequirement requirement)
        {
            var notContainsPaths = new List<string>()
            {
                "api/redirect-edevlet-login",
                "api/edevlet-login-result",
            };
            var requestPath = HttpContext.Request.Path.Value;
            if (!requestPath.StartsWith("/api") ||
                requestPath.Contains("api/redirect-edevlet-login") ||
                requestPath.Contains("api/edevlet-login-result"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            Claim ipClaim = context.User.FindFirst(claim => claim.Type == "userIp");

            // No claim existing set and and its configured as optional so skip the check
            if (ipClaim == null && !requirement.IpClaimRequired)
            {
                // Optional claims (IsClaimRequired=false and no "ipaddress" in the claims principal) won't call context.Fail()
                // This allows next Handle to succeed. If we call Fail() the access will be denied, even if handlers
                // evaluated after this one do succeed
                return Task.CompletedTask;
            }

            if (ipClaim == null || ipClaim.Value == HttpContext.Connection.RemoteIpAddress?.ToString())
            {
                context.Succeed(requirement);
            }
            else
            {
                // Only call fail, to guarantee a failure, even if further handlers may succeed
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
