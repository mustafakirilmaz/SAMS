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
    /// İşletme Projesi İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/businessProjects")]
    public class BusinessProjectController : BaseController
    {
        private readonly IBusinessProjectBusinessService businessProjectService;

        /// <summary>
        /// İşletme Projesi işlemleri ctor
        /// </summary>
        /// <param name="businessProjectService"></param>
        public BusinessProjectController(IBusinessProjectBusinessService businessProjectService)
        {
            this.businessProjectService = businessProjectService;
        }

        /// <summary>
        /// Id'ye göre işletme projesi getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<BusinessProjectDto>> GetBusinessProjectById(long id)
        {
            return await businessProjectService.GetBusinessProjectById(id);
        }

        /// <summary>
        /// İşletme Projesi ekleme
        /// </summary>
        /// <param name="businessProjectDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateBusinessProject([FromBody] BusinessProjectDto businessProjectDto)
        {
            return await businessProjectService.AddBusinessProject(businessProjectDto);
        }

        /// <summary>
        /// İşletme Projesi güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessProjectDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateBusinessProject(long id, [FromBody] BusinessProjectDto businessProjectDto)
        {
            businessProjectDto.Id = id;
            return await businessProjectService.UpdateBusinessProject(businessProjectDto);
        }

        /// <summary>
        /// Kayıtlı işletme projesiları arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<BusinessProjectGridDto>>> SearchBusinessProjects([FromQuery] QueryFilter<BusinessProjectFilterDto> queryFilter)
        {
            return await businessProjectService.SearchBusinessProjects(queryFilter);
        }

        /// <summary>
        /// İşletme Projesi silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteBusinessProject(long id)
        {
            return await businessProjectService.DeleteBusinessProject(id);
        }
    }
}
