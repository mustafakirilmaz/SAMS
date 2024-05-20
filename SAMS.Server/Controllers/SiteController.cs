using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAMS.Data.Dtos;
using SAMS.Infrastructure.Attributes;
using SAMS.Infrastructure.Constants;
using SAMS.Infrastructure.Controller;
using SAMS.Infrastructure.Models;
using SAMS.Server.ServiceContracts;

namespace SAMS.Server.Controllers
{
    /// <summary>
    /// Site İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/sites")]
    public class SiteController : BaseController
    {
        private readonly ISiteBusinessService siteService;

        /// <summary>
        /// Site işlemleri ctor
        /// </summary>
        /// <param name="siteService"></param>
        public SiteController(ISiteBusinessService siteService)
        {
            this.siteService = siteService;
        }

        /// <summary>
        /// Id'ye göre site getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<SiteDto>> GetSiteById(long id)
        {
            return await siteService.GetSiteById(id);
        }

        /// <summary>
        /// Site ekleme
        /// </summary>
        /// <param name="siteDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateSite([FromBody] SiteDto siteDto)
        {
            return await siteService.AddSite(siteDto);
        }

        /// <summary>
        /// Site güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="siteDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateSite(long id, [FromBody] SiteDto siteDto)
        {
            siteDto.Id = id;
            return await siteService.UpdateSite(siteDto);
        }

        /// <summary>
        /// Kayıtlı siteleri arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<SiteGridDto>>> SearchSites([FromQuery] QueryFilter<SiteFilterDto> queryFilter)
        {
            return await siteService.SearchSites(queryFilter);
        }

        /// <summary>
        /// Site silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteSite(long id)
        {
            return await siteService.DeleteSite(id);
        }

        ///// <summary>
        ///// Sisteme login olan kullanıcının site bilgilerini getirir(Gelen token bilgisine göre sitenin bilgilerini döner)
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("current")]
        //public async Task<ServiceResult<SiteDto>> GetCurrentSite()
        //{
        //    return await siteService.GetCurrentSite();
        //}
    }
}
