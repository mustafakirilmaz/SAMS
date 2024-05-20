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
    /// Eşit Gider İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/equalExpenses")]
    public class EqualExpenseController : BaseController
    {
        private readonly IEqualExpenseBusinessService equalExpenseService;

        /// <summary>
        /// Eşit Gider işlemleri ctor
        /// </summary>
        /// <param name="equalExpenseService"></param>
        public EqualExpenseController(IEqualExpenseBusinessService equalExpenseService)
        {
            this.equalExpenseService = equalExpenseService;
        }

        /// <summary>
        /// Id'ye göre eşit gider getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<EqualExpenseDto>> GetEqualExpenseById(long id)
        {
            return await equalExpenseService.GetEqualExpenseById(id);
        }

        /// <summary>
        /// Eşit Gider ekleme
        /// </summary>
        /// <param name="equalExpenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateEqualExpense([FromBody] EqualExpenseDto equalExpenseDto)
        {
            return await equalExpenseService.AddEqualExpense(equalExpenseDto);
        }

        /// <summary>
        /// Eşit Gider güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equalExpenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateEqualExpense(long id, [FromBody] EqualExpenseDto equalExpenseDto)
        {
            equalExpenseDto.Id = id;
            return await equalExpenseService.UpdateEqualExpense(equalExpenseDto);
        }

        /// <summary>
        /// Kayıtlı eşit giderları arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<EqualExpenseGridDto>>> SearchEqualExpenses([FromQuery] QueryFilter<EqualExpenseFilterDto> queryFilter)
        {
            return await equalExpenseService.SearchEqualExpenses(queryFilter);
        }

        /// <summary>
        /// Eşit Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteEqualExpense(long id)
        {
            return await equalExpenseService.DeleteEqualExpense(id);
        }
    }
}
