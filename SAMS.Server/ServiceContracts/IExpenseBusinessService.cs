using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Gider işlemleri servisi
    /// </summary>
    public interface IExpenseBusinessService
    {

        /// <summary>
        /// Gider ekle
        /// </summary>
        /// <param name="expenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddExpense(ExpenseDto expenseDto);

        /// <summary>
        /// Gider güncelle
        /// </summary>
        /// <param name="expenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateExpense(ExpenseDto expenseDto);

        /// <summary>
        /// Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteExpense(long id);

        /// <summary>
        /// Id'ye göre gider getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<ExpenseDto>> GetExpenseById(long id);


        ///// <summary>
        ///// Gider arama
        ///// </summary>
        ///// <param name="queryFilter"></param>
        ///// <returns></returns>

        Task<ServiceResult<List<ExpenseGridDto>>> SearchExpenses(QueryFilter<ExpenseFilterDto> queryFilter);
    }
}
