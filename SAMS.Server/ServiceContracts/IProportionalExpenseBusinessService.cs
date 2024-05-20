using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Oransal Gider işlemleri servisi
    /// </summary>
    public interface IProportionalExpenseBusinessService
    {

        /// <summary>
        /// Oransal Gider ekle
        /// </summary>
        /// <param name="proportionalExpenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddProportionalExpense(ProportionalExpenseDto proportionalExpenseDto);

        /// <summary>
        /// Oransal Gider güncelle
        /// </summary>
        /// <param name="proportionalExpenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateProportionalExpense(ProportionalExpenseDto proportionalExpenseDto);

        /// <summary>
        /// Oransal Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteProportionalExpense(long id);

        /// <summary>
        /// Id'ye göre oransal gider getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<ProportionalExpenseDto>> GetProportionalExpenseById(long id);


        /// <summary>
        /// Oransal Gider arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        Task<ServiceResult<List<ProportionalExpenseGridDto>>> SearchProportionalExpenses(QueryFilter<ProportionalExpenseFilterDto> queryFilter);
    }
}
