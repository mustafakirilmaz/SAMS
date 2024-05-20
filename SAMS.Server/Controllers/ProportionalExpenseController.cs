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
    /// Oransal Gider İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/proportionalExpenses")]
    public class ProportionalExpenseController : BaseController
    {
        private readonly IProportionalExpenseBusinessService proportionalExpenseService;

        /// <summary>
        /// Oransal Gider işlemleri ctor
        /// </summary>
        /// <param name="proportionalExpenseService"></param>
        public ProportionalExpenseController(IProportionalExpenseBusinessService proportionalExpenseService)
        {
            this.proportionalExpenseService = proportionalExpenseService;
        }

        /// <summary>
        /// Id'ye göre oransal gider getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<ProportionalExpenseDto>> GetProportionalExpenseById(long id)
        {
            return await proportionalExpenseService.GetProportionalExpenseById(id);
        }

        /// <summary>
        /// Oransal Gider ekleme
        /// </summary>
        /// <param name="proportionalExpenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateProportionalExpense([FromBody] ProportionalExpenseDto proportionalExpenseDto)
        {
            return await proportionalExpenseService.AddProportionalExpense(proportionalExpenseDto);
        }

        /// <summary>
        /// Oransal Gider güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="proportionalExpenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateProportionalExpense(long id, [FromBody] ProportionalExpenseDto proportionalExpenseDto)
        {
            proportionalExpenseDto.Id = id;
            return await proportionalExpenseService.UpdateProportionalExpense(proportionalExpenseDto);
        }

        /// <summary>
        /// Kayıtlı oransal giderları arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<ProportionalExpenseGridDto>>> SearchProportionalExpenses([FromQuery] QueryFilter<ProportionalExpenseFilterDto> queryFilter)
        {
            return await proportionalExpenseService.SearchProportionalExpenses(queryFilter);
        }

        /// <summary>
        /// Oransal Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteProportionalExpense(long id)
        {
            return await proportionalExpenseService.DeleteProportionalExpense(id);
        }
    }
}
