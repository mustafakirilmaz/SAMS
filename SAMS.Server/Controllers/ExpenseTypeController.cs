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
    /// Gider Türü İşlemleri
    /// </summary>
    [ApiController]
    [Route("api/expenseTypes")]
    public class ExpenseTypeController : BaseController
    {
        private readonly IExpenseTypeBusinessService expenseTypeService;

        /// <summary>
        /// Gider Türü işlemleri ctor
        /// </summary>
        /// <param name="expenseTypeService"></param>
        public ExpenseTypeController(IExpenseTypeBusinessService expenseTypeService)
        {
            this.expenseTypeService = expenseTypeService;
        }

        /// <summary>
        /// Id'ye göre Gider Türü getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<ExpenseTypeDto>> GetExpenseTypeById(long id)
        {
            return await expenseTypeService.GetExpenseTypeById(id);
        }

        /// <summary>
        /// Gider Türü ekleme
        /// </summary>
        /// <param name="expenseTypeDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateExpenseType([FromBody] ExpenseTypeDto expenseTypeDto)
        {
            return await expenseTypeService.AddExpenseType(expenseTypeDto);
        }

        /// <summary>
        /// Gider Türü güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expenseTypeDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateExpenseType(long id, [FromBody] ExpenseTypeDto expenseTypeDto)
        {
            expenseTypeDto.Id = id;
            return await expenseTypeService.UpdateExpenseType(expenseTypeDto);
        }

        /// <summary>
        /// Kayıtlı Gider Türlerini arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<ExpenseTypeGridDto>>> SearchExpenseTypes([FromQuery] QueryFilter<ExpenseTypeFilterDto> queryFilter)
        {
            return await expenseTypeService.SearchExpenseTypes(queryFilter);
        }

        /// <summary>
        /// Gider Türü silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteExpenseType(long id)
        {
            return await expenseTypeService.DeleteExpenseType(id);
        }
    }
}
