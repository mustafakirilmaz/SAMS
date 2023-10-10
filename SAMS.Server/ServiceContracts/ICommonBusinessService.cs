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
    }
}
