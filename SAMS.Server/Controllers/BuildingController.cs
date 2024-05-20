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
    /// Bina İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/buildings")]
    public class BuildingController : BaseController
    {
        private readonly IBuildingBusinessService buildingService;

        /// <summary>
        /// Bina işlemleri ctor
        /// </summary>
        /// <param name="buildingService"></param>
        public BuildingController(IBuildingBusinessService buildingService)
        {
            this.buildingService = buildingService;
        }

        /// <summary>
        /// Id'ye göre bina getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<BuildingDto>> GetBuildingById(long id)
        {
            return await buildingService.GetBuildingById(id);
        }

        /// <summary>
        /// Bina ekleme
        /// </summary>
        /// <param name="buildingDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateBuilding([FromBody] BuildingDto buildingDto)
        {
            return await buildingService.AddBuilding(buildingDto);
        }

        /// <summary>
        /// Bina güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buildingDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateBuilding(long id, [FromBody] BuildingDto buildingDto)
        {
            buildingDto.Id = id;
            return await buildingService.UpdateBuilding(buildingDto);
        }

        /// <summary>
        /// Kayıtlı binaları arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<BuildingGridDto>>> SearchBuildings([FromQuery] QueryFilter<BuildingFilterDto> queryFilter)
        {
            return await buildingService.SearchBuildings(queryFilter);
        }

        /// <summary>
        /// Bina silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteBuilding(long id)
        {
            return await buildingService.DeleteBuilding(id);
        }
    }
}
