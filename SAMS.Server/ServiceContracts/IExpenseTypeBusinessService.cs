using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Gider Türü işlemleri servisi
    /// </summary>
    public interface IExpenseTypeBusinessService
    {

        /// <summary>
        /// Gider Türü ekle
        /// </summary>
        /// <param name="expenseTypeDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddExpenseType(ExpenseTypeDto expenseTypeDto);

        /// <summary>
        /// Gider Türü güncelle
        /// </summary>
        /// <param name="expenseTypeDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateExpenseType(ExpenseTypeDto expenseTypeDto);

        /// <summary>
        /// Gider Türü silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteExpenseType(long id);

        /// <summary>
        /// Id'ye göre Gider Türü getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<ExpenseTypeDto>> GetExpenseTypeById(long id);

        /// <summary>
        /// Gider Türü arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<ServiceResult<List<ExpenseTypeGridDto>>> SearchExpenseTypes(QueryFilter<ExpenseTypeFilterDto> queryFilter);
    }
}
