using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Eşit Gider işlemleri servisi
    /// </summary>
    public interface IEqualExpenseBusinessService
    {

        /// <summary>
        /// Eşit Gider ekle
        /// </summary>
        /// <param name="equalExpenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddEqualExpense(EqualExpenseDto equalExpenseDto);

        /// <summary>
        /// Eşit Gider güncelle
        /// </summary>
        /// <param name="equalExpenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateEqualExpense(EqualExpenseDto equalExpenseDto);

        /// <summary>
        /// Eşit Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteEqualExpense(long id);

        /// <summary>
        /// Id'ye göre eşit gider getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<EqualExpenseDto>> GetEqualExpenseById(long id);


        /// <summary>
        /// Eşit Gider arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        Task<ServiceResult<List<EqualExpenseGridDto>>> SearchEqualExpenses(QueryFilter<EqualExpenseFilterDto> queryFilter);
    }
}
