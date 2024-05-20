using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// İşletme Projesi işlemleri servisi
    /// </summary>
    public interface IBusinessProjectBusinessService
    {

        /// <summary>
        /// İşletme Projesi ekle
        /// </summary>
        /// <param name="businessProjectDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddBusinessProject(BusinessProjectDto businessProjectDto);

        /// <summary>
        /// İşletme Projesi güncelle
        /// </summary>
        /// <param name="businessProjectDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateBusinessProject(BusinessProjectDto businessProjectDto);

        /// <summary>
        /// İşletme Projesi silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteBusinessProject(long id);

        /// <summary>
        /// Id'ye göre işletme projesi getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<BusinessProjectDto>> GetBusinessProjectById(long id);


        /// <summary>
        /// İşletme Projesi arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        Task<ServiceResult<List<BusinessProjectGridDto>>> SearchBusinessProjects(QueryFilter<BusinessProjectFilterDto> queryFilter);
    }
}
