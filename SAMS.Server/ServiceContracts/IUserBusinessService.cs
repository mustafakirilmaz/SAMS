using SAMS.Data.Dtos;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Kullanıcı işlemleri servisi
    /// </summary>
    public interface IUserBusinessService
    {

        /// <summary>
        /// Kullanıcı ekle
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> AddUser(UserDto userDto);

        /// <summary>
        /// Kullanıcı güncelle
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateUser(UserDto userDto);

        /// <summary>
        /// Kullanıcı silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteUser(long id);

        /// <summary>
        /// Id'ye göre kullanıcı getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<UserDto>> GetUserById(long id);

        /// <summary>
        /// Sisteme login olan kullanıcının kendi bilgilerini getirir(Gelen token bilgisine göre kullanıcının bilgilerini döner)
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<UserDto>> GetCurrentUser();

        /// <summary>
        /// Kullanıcı arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<ServiceResult<List<UserGridDto>>> SearchUsers(QueryFilter<UserFilterDto> queryFilter);
    }
}
