using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAMS.Infrastructure.Controller;
using SAMS.Infrastructure.Models;
using SAMS.Server.ServiceContracts;

namespace SAMS.Server.Controllers
{
    /// <summary>
    /// Ortak verileri getir
    /// </summary>
    [ApiController]
    [Route("api/common")]
    public class CommonController : BaseController
    {
        private readonly ICommonBusinessService commonService;

        /// <summary>
        /// Ortak verileri getir ctor
        /// </summary>
        /// <param name="commonService"></param>
        public CommonController(ICommonBusinessService commonService)
        {
            this.commonService = commonService;
        }

        /// <summary>
        /// Site ayakta mı kontrolü
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string CheckHealth()
        {
            return "Site is online";
        }

        #region Enums
        /// <summary>
        /// Örnek enumları Getir
        /// </summary>
        /// <returns></returns>
        [HttpGet("device-enums")]
        [AllowAnonymous]
        public async Task<ServiceResult<List<SelectItem>>> GetSampleEnum()
        {
            return await commonService.GetSampleEnum();
        }
        #endregion

        #region Models
        /// <summary>
        /// Sistemdeki rolleri getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public async Task<ServiceResult<List<SelectItem>>> GetRoles()
        {
            return await commonService.GetRoles();
        }

        /// <summary>
        /// Ad soyad'a göre kullanıcı arama
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<ServiceResult<List<SelectItem>>> GetUsers(string searchTerm)
        {
            return await commonService.GetUsers(searchTerm);
        }
        #endregion
    }
}
