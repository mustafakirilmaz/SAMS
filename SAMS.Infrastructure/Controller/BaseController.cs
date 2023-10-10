using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMS.Infrastructure.Attributes;

namespace SAMS.Infrastructure.Controller
{
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
    }
}
