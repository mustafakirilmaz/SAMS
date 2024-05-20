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

        /// <summary>
        /// Şehirleri Alma
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("cities")]
		public async Task<ServiceResult> GetCities()
		{
			return await commonService.GetCities();
		}

        /// <summary>
        /// İlçeleri Alma
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("towns")]
		public async Task<ServiceResult> GetTowns(int cityCode)
		{
			return await commonService.GetTowns(cityCode);
		}

        /// <summary>
        /// Mahalleleri Alma
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("districts")]
		public async Task<ServiceResult> GetDistricts(int townCode)
		{
			return await commonService.GetDistricts(townCode);
        }

        /// <summary>
        /// Tüm Siteleri Döner
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("sites")]
        public async Task<ServiceResult> GetSites()
        {
            return await commonService.GetSites();
        }

        /// <summary>
        /// Tüm Binaları Döner
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("buildings")]
        public async Task<ServiceResult> GetBuildings()
        {
            return await commonService.GetBuildings();
        }

        /// <summary>
        /// Tüm Eşit Gider Türlerini Döner
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("equalexpensetypes")]
        public async Task<ServiceResult> GetEqualExpenseTypes()
        {
            return await commonService.GetEqualExpenseTypes();
        }

        /// <summary>
        /// Tüm Oransal Gider Türlerini Döner
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("proportionalexpensetypes")]
        public async Task<ServiceResult> GetProportionalExpenseTypes()
        {
            return await commonService.GetProportionalExpenseTypes();
        }

        /// <summary>
        /// Tüm Demirbaş Gider Türlerini Döner
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("fixtureexpensetypes")]
        public async Task<ServiceResult> GetFixtureExpenseTypes()
        {
            return await commonService.GetFixtureExpenseTypes();
        }

        /// <summary>
        /// Tüm İşletme Projelerini Döner
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("businessprojects")]
        public async Task<ServiceResult> GetBusinessProjects()
        {
            return await commonService.GetBusinessProjects();
        }

        #endregion
    }
}
