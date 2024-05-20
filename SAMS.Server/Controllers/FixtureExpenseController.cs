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
    /// Demirbaş Gideri İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/fixtureExpenses")]
    public class FixtureExpenseController : BaseController
    {
        private readonly IFixtureExpenseBusinessService fixtureExpenseService;

        /// <summary>
        /// Demirbaş Gideri işlemleri ctor
        /// </summary>
        /// <param name="fixtureExpenseService"></param>
        public FixtureExpenseController(IFixtureExpenseBusinessService fixtureExpenseService)
        {
            this.fixtureExpenseService = fixtureExpenseService;
        }

        /// <summary>
        /// Id'ye göre Demirbaş Gideri getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<FixtureExpenseDto>> GetFixtureExpenseById(long id)
        {
            return await fixtureExpenseService.GetFixtureExpenseById(id);
        }

        /// <summary>
        /// Demirbaş Gideri ekleme
        /// </summary>
        /// <param name="fixtureExpenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateFixtureExpense([FromBody] FixtureExpenseDto fixtureExpenseDto)
        {
            return await fixtureExpenseService.AddFixtureExpense(fixtureExpenseDto);
        }

        /// <summary>
        /// Demirbaş Gideri güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fixtureExpenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateFixtureExpense(long id, [FromBody] FixtureExpenseDto fixtureExpenseDto)
        {
            fixtureExpenseDto.Id = id;
            return await fixtureExpenseService.UpdateFixtureExpense(fixtureExpenseDto);
        }

        /// <summary>
        /// Kayıtlı Demirbaş Gideri arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<FixtureExpenseGridDto>>> SearchFixtureExpenses([FromQuery] QueryFilter<FixtureExpenseFilterDto> queryFilter)
        {
            return await fixtureExpenseService.SearchFixtureExpenses(queryFilter);
        }

        /// <summary>
        /// Demirbaş Gideri silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteFixtureExpense(long id)
        {
            return await fixtureExpenseService.DeleteFixtureExpense(id);
        }
    }
}
