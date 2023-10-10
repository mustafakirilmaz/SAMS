using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SAMS.Data.Dtos;
using SAMS.Data.Dtos.User;
using SAMS.Infrastructure.Attributes;
using SAMS.Infrastructure.Constants;
using SAMS.Infrastructure.Controller;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.Models;
using SAMS.Infrastructure.WebToken;
using SAMS.Server.ServiceContracts;

namespace SAMS.Server.Controllers
{
    /// <summary>
    /// Kullanıcı hesap işlemleri
    /// </summary>
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : BaseController
    {
        private readonly IJwtHelper jwtHelper;
        private readonly IAccountBusinessService accountService;

        /// <summary>
        /// Kullanıcı hesap işlemleri ctor
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="jwtHelper"></param>
        public AccountController(IAccountBusinessService accountService, IJwtHelper jwtHelper)
        {
            this.jwtHelper = jwtHelper;
            this.accountService = accountService;
        }

        /// <summary>
        /// Sisteme giriş yapma
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ServiceResult<UserInfoDto>> Login(LoginDto model)
        {
            var result = await accountService.Login(model);
            if (result.IsSuccess())
            {
                result.Data.UserIp = HttpContext.Connection.RemoteIpAddress.ToString();
                result.Data = jwtHelper.GenerateJwtToken(result.Data);

                var authorization = result.Data.Token;
                HttpContext.Request.Headers.Add("Authorization", authorization);
            }

            return result;
        }

        /// <summary>
        /// Seçili rolü ayarla
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("set-selected-role")]
        public async Task<ServiceResult<UserInfoDto>> SetSelectedRole(UserInfoRoleDto role)
        {
            var result = new ServiceResult<UserInfoDto>(null);
            if (role != null)
            {
                result.Data = jwtHelper.GetCurrentUser();
                if (!result.Data.Roles.Any(d => d.Id == role.Id))
                {
                    return await Task.FromResult(new ServiceResult<UserInfoDto>(null, "Yetkisiz işlem girişimi!", ResultType.Error));
                }
                result.Data = jwtHelper.GenerateJwtToken(result.Data, role);
                return await Task.FromResult(result);
            }

            return await Task.FromResult(new ServiceResult<UserInfoDto>(null, "Seçili rol yok", ResultType.Error));
        }

        /// <summary>
        /// Şifre değiştirme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("change-password")]
        [AllowAnonymous]
        public async Task<ServiceResult<UserInfoDto>> ChangePassword(ChangePasswordDto model)
        {
            return await accountService.ChangePassword(model);
        }

		/// <summary>
		/// Profilden şifre değiştirme
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("change-password-profile")]
		public async Task<ServiceResult<UserInfoDto>> ChangePasswordProfile(ChangePasswordProfileDto model)
		{
			return await accountService.ChangePasswordProfile(model);
		}

		/// <summary>
		/// Şifre değiştirme mobil
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("change-password-mobile")]
        [AllowAnonymous]
        public async Task<ServiceResult<UserInfoDto>> ChangePasswordMobile(ChangePasswordMobileDto model)
        {
            return await accountService.ChangePasswordMobile(model);
        }

        /// <summary>
        /// Şifremi unuttum
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("forget-password")]
        [AllowAnonymous]
        public async Task<ServiceResult> ForgetPassword(ForgetPasswordDto model)
        {
            return await accountService.ForgetPassword(model);
        }

        /// <summary>
        /// Şifremi unuttum mobil
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("forget-password-mobile")]
        [AllowAnonymous]
        public async Task<ServiceResult> ForgetPasswordMobile(ForgetPasswordDto model)
        {
            return await accountService.ForgetPasswordMobile(model);
        }

        /// <summary>
        /// Şifre değişimi için guid kontrol
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("check-guid/{guid}")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ServiceResult<bool>> CheckGuid(string guid)
        {
            return await accountService.CheckGuid(guid);
        }

        /// <summary>
        /// Şifre değişimi için otp kod kontrol
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("check-user-otp-code")]
        [AllowAnonymous]
        public async Task<ServiceResult<bool>> CheckUserOtpCode(CheckUserOtpDto model)
        {
            return await accountService.CheckUserOtpCode(model);
        }

        /// <summary>
        /// Otp kod ile kullanıcı aktifleştirme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("activate-user-otp-code")]
        [AllowAnonymous]
        public async Task<ServiceResult<bool>> ActivateUserWithOtpCode(CheckUserOtpDto model)
        {
            return await accountService.ActivateUserWithOtpCode(model);
        }

        /// <summary>
        /// Başka kullanıcı hesabına geçiş yap
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Roles(RoleNames.Admin)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ServiceResult<UserInfoDto>> LoginAnotherUserAccount(string email)
        {
            var result = await accountService.LoginAnotherUserAccount(email);
            if (result.IsSuccess())
            {
                result.Data.UserIp = HttpContext.Connection.RemoteIpAddress.ToString();
                result.Data = jwtHelper.GenerateJwtToken(result.Data);
            }

            return result;
        }

        /// <summary>
        /// Kullanıcı profil güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("profile/{id}")]
        public async Task<ServiceResult<long?>> UpdateProfile(long id, [FromBody] UserDto userDto)
        {
            userDto.Id = id;
            return await accountService.UpdateProfile(userDto);
        }

        /// <summary>
        /// Sisteme üye olma
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ServiceResult<long?>> Register(RegisterUserDto model)
        {
            return await accountService.Register(model);
        }
    }
}
