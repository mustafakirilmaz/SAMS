using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Bina işlemleri servisi
    /// </summary>
    public interface IBuildingBusinessService
    {

        /// <summary>
        /// Bina ekle
        /// </summary>
        /// <param name="buildingDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddBuilding(BuildingDto buildingDto);

        /// <summary>
        /// Bina güncelle
        /// </summary>
        /// <param name="buildingDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateBuilding(BuildingDto buildingDto);

        /// <summary>
        /// Bina silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteBuilding(long id);

        /// <summary>
        /// Id'ye göre bina getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<BuildingDto>> GetBuildingById(long id);


        ///// <summary>
        ///// Bina arama
        ///// </summary>
        ///// <param name="queryFilter"></param>
        ///// <returns></returns>

        Task<ServiceResult<List<BuildingGridDto>>> SearchBuildings(QueryFilter<BuildingFilterDto> queryFilter);
    }
}
