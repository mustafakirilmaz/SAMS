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
    /// Gider İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/expenses")]
    public class ExpenseController : BaseController
    {
        private readonly IExpenseBusinessService expenseService;

        /// <summary>
        /// Gider işlemleri ctor
        /// </summary>
        /// <param name="expenseService"></param>
        public ExpenseController(IExpenseBusinessService expenseService)
        {
            this.expenseService = expenseService;
        }

        /// <summary>
        /// Id'ye göre dider getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<ExpenseDto>> GetExpenseById(long id)
        {
            return await expenseService.GetExpenseById(id);
        }

        /// <summary>
        /// Gider ekleme
        /// </summary>
        /// <param name="expenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateExpense([FromBody] ExpenseDto expenseDto)
        {
            return await expenseService.AddExpense(expenseDto);
        }

        /// <summary>
        /// Gider güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expenseDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateExpense(long id, [FromBody] ExpenseDto expenseDto)
        {
            expenseDto.Id = id;
            return await expenseService.UpdateExpense(expenseDto);
        }

        /// <summary>
        /// Kayıtlı giderları arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<ExpenseGridDto>>> SearchExpenses([FromQuery] QueryFilter<ExpenseFilterDto> queryFilter)
        {
            return await expenseService.SearchExpenses(queryFilter);
        }

        /// <summary>
        /// Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteExpense(long id)
        {
            return await expenseService.DeleteExpense(id);
        }
    }
}
