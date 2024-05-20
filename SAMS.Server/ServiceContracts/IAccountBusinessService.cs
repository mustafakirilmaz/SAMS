using System.Threading.Tasks;
using SAMS.Data.Dtos;
using SAMS.Data.Dtos.User;
using SAMS.Infrastructure.Models;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Kullanıcı hesap işlemleri servisi
    /// </summary>
    public interface IAccountBusinessService
    {
        /// <summary>
        /// Sisteme giriş
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ServiceResult<UserInfoDto>> Login(LoginDto input);

        /// <summary>
        /// Şifremi unuttum
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResult> ForgetPassword(ForgetPasswordDto model);

        /// <summary>
        /// Şifremi unuttum mobil
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResult> ForgetPasswordMobile(ForgetPasswordDto model);

        /// <summary>
        /// Şifre değiştir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResult<UserInfoDto>> ChangePassword(ChangePasswordDto model);

        /// <summary>
        /// Şifre değiştir mobil
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResult<UserInfoDto>> ChangePasswordMobile(ChangePasswordMobileDto model);

        /// <summary>
        /// Profilden şifre değiştir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResult<UserInfoDto>> ChangePasswordProfile(ChangePasswordProfileDto model);

        /// <summary>
        /// Kullanıcı guid kontrolü
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> CheckGuid(string guid);

        /// <summary>
        /// Kullanıcı otp kod kontrolü
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> CheckUserOtpCode(CheckUserOtpDto model);

        /// <summary>
        /// OTP kod ile kullanıcı aktifleştirme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> ActivateUserWithOtpCode(CheckUserOtpDto model);

        /// <summary>
        /// Başka kullanıcının hesabına geçiş yapma
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ServiceResult<UserInfoDto>> LoginAnotherUserAccount(string email);

        /// <summary>
        /// Profil güncelleme
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<ServiceResult<long?>> UpdateProfile(UserDto userDto);

        ///// <summary>
        ///// Kullanıcı üye olma
        ///// </summary>
        ///// <param name="userDto"></param>
        ///// <returns></returns>
        //Task<ServiceResult<long?>> Register(RegisterUserDto userDto);

    }
}
