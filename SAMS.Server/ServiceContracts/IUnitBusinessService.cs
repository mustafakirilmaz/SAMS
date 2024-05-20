using SAMS.Data;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;
using SAMS.Data.Dtos;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Bağımsız Bölüm işlemleri servisi
    /// </summary>
    public interface IUnitBusinessService
    {

        /// <summary>
        /// Bağımsız Bölüm ekle
        /// </summary>
        /// <param name="unitDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddUnit(UnitDto unitDto);

        /// <summary>
        /// Bağımsız Bölüm güncelle
        /// </summary>
        /// <param name="unitDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateUnit(UnitDto unitDto);

        /// <summary>
        /// Bağımsız Bölüm silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteUnit(long id);

        /// <summary>
        /// Id'ye göre bağımsız bölüm getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<UnitDto>> GetUnitById(long id);

        /// <summary>
        /// Bağımsız Bölüm arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<ServiceResult<List<UnitGridDto>>> SearchUnits(QueryFilter<UnitFilterDto> queryFilter);
    }
}
