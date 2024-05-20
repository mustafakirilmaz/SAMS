using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Ortak kullanılan servis
    /// </summary>
    public interface ICommonBusinessService
    {
        /// <summary>
        /// Rolleri getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetRoles();

        /// <summary>
        /// Cihaz enumları Getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetSampleEnum();

        /// <summary>
        /// Ad soyad'a göre kullanıcı arama
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetUsers(string searchTerm);

        /// <summary>
        /// İlleri getir
        /// </summary>
        /// <returns></returns>
		Task<ServiceResult<List<SelectItem>>> GetCities();

        /// <summary>
        /// İlçeleri Getir
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
		Task<ServiceResult<List<SelectItem>>> GetTowns(int cityCode);

        /// <summary>
        /// Mahalleleri getir
        /// </summary>
        /// <param name="townCode"></param>
        /// <returns></returns>
		Task<ServiceResult<List<SelectItem>>> GetDistricts(int townCode);

        /// <summary>
        /// Siteleri getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetSites();

        /// <summary>
        /// Binaları getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetBuildings();

        /// <summary>
        /// Eşit Gider Türlerini Getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetEqualExpenseTypes();

        /// <summary>
        /// Oransal Gider Türlerini Getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetProportionalExpenseTypes();

        /// <summary>
        /// Demirbaş Gider Türlerini Getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetFixtureExpenseTypes();

        /// <summary>
        /// İşletme Projelerini Getir
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<SelectItem>>> GetBusinessProjects();
    }
}
