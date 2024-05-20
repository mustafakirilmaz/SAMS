using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAMS.Infrastructure.Attributes;
using SAMS.Infrastructure.Constants;
using SAMS.Infrastructure.Controller;
using SAMS.Infrastructure.Models;
using SAMS.Server.ServiceContracts;
using SAMS.Data;
using SAMS.Data.Dtos;

namespace SAMS.Server.Controllers
{
    /// <summary>
    /// Bağımsız Bölüm İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/units")]
    public class UnitController : BaseController
    {
        private readonly IUnitBusinessService unitService;

        /// <summary>
        /// Bağımsız Bölüm işlemleri ctor
        /// </summary>
        /// <param name="unitService"></param>
        public UnitController(IUnitBusinessService unitService)
        {
            this.unitService = unitService;
        }

        /// <summary>
        /// Id'ye göre bağımsız bölüm getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<UnitDto>> GetUnitById(long id)
        {
            return await unitService.GetUnitById(id);
        }

        /// <summary>
        /// Bağımsız Bölüm ekleme
        /// </summary>
        /// <param name="unitDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateUnit([FromBody] UnitDto unitDto)
        {
            return await unitService.AddUnit(unitDto);
        }

        /// <summary>
        /// Bağımsız Bölüm güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unitDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateUnit(long id, [FromBody] UnitDto unitDto)
        {
            unitDto.Id = id;
            return await unitService.UpdateUnit(unitDto);
        }

        /// <summary>
        /// Kayıtlı bağımsız bölümleri arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<UnitGridDto>>> SearchUnits([FromQuery] QueryFilter<UnitFilterDto> queryFilter)
        {
            return await unitService.SearchUnits(queryFilter);
        }

        /// <summary>
        /// Bağımsız Bölüm silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteUnit(long id)
        {
            return await unitService.DeleteUnit(id);
        }
    }
}
