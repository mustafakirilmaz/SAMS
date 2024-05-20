using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Demirbaş Gideri işlemleri servisi
    /// </summary>
    public interface IFixtureExpenseBusinessService
    {

        /// <summary>
        /// Demirbaş Gideri ekle
        /// </summary>
        /// <param name="fixtureExpenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddFixtureExpense(FixtureExpenseDto fixtureExpenseDto);

        /// <summary>
        /// Oransal Gider güncelle
        /// </summary>
        /// <param name="fixtureExpenseDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateFixtureExpense(FixtureExpenseDto fixtureExpenseDto);

        /// <summary>
        /// Oransal Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteFixtureExpense(long id);

        /// <summary>
        /// Id'ye göre Demirbaş Gideri getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<FixtureExpenseDto>> GetFixtureExpenseById(long id);


        /// <summary>
        /// Oransal Gider arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        Task<ServiceResult<List<FixtureExpenseGridDto>>> SearchFixtureExpenses(QueryFilter<FixtureExpenseFilterDto> queryFilter);
    }
}
