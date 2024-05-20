using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Site işlemleri servisi
    /// </summary>
    public interface ISiteBusinessService
    {

        /// <summary>
        /// Site ekle
        /// </summary>
        /// <param name="siteDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddSite(SiteDto siteDto);

        /// <summary>
        /// Site güncelle
        /// </summary>
        /// <param name="siteDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateSite(SiteDto siteDto);

        /// <summary>
        /// Site silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteSite(long id);

        /// <summary>
        /// Id'ye göre site getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<SiteDto>> GetSiteById(long id);

        ///// <summary>
        ///// Sisteme login olan kullanıcının site bilgilerini getirir. (Gelen token bilgisine göre sitenin bilgilerini döner)
        ///// </summary>
        ///// <returns></returns>
        //Task<ServiceResult<SiteDto>> GetCurrentSite();

        /// <summary>
        /// Site arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<ServiceResult<List<SiteGridDto>>> SearchSites(QueryFilter<SiteFilterDto> queryFilter);
    }
}
