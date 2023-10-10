using SAMS.Common.Helpers;
using SAMS.Data;
using SAMS.Data.Dtos;
using SAMS.Data.Dtos.User;
using SAMS.DataAccess;
using SAMS.Helpers;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.Models;
using SAMS.Infrastructure.WebToken;
using SAMS.Server.ServiceContracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAMS.Infrastructure.Constants;
using SAMS.Common.Extensions;

namespace SAMS.Server.Services
{
    /// <summary>
    /// Kullanıcı hesap işlemleri servisi
    /// </summary>
    public class AccountBusinessService : IAccountBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Kullanıcı hesap işlemleri servisi ctor
        /// </summary>
        public AccountBusinessService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Sisteme giriş
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserInfoDto>> Login(LoginDto model)
        {
            var user = await UnitOfWork.GetRepository<User>().Get(d => d.Email == model.Email && !d.IsDeleted);
            return await LoginCommon(user, model);
        }

        /// <summary>
        /// Şifre değiştir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserInfoDto>> ChangePassword(ChangePasswordDto model)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            var user = await userRepo.Get(d => d.PasswordResetGuid == model.Guid);

            if (user == null)
            {
                return new ServiceResult<UserInfoDto>(null, "Kullanıcı bulunamadı", ResultType.Error);
            }

            var message = EmailTemplateHelper.GenerateChangePasswordSuccessMailContent(user.Name, user.Surname);
            await EmailHelper.SendMail(user.Email, "Şifre Değişimi", message);

            var password = PasswordHelper.HashPassword(model.NewPassword);
            user.Password = password;
            user.PasswordResetGuid = null;
            user.PasswordResetOtpCode = null;
            user.IsPasswordChangeNextLogin = false;
            user.IsActive = true;
            userRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            var userInfo = new UserInfoDto()
            {
                Email = user.Email,
                Fullname = string.Format("{0} {1}", user.Name, user.Surname),
                Name = user.Name,
                Surname = user.Surname,
                Id = user.Id,
                ChangePassword = user.IsPasswordChangeNextLogin,
            };

            return new ServiceResult<UserInfoDto>(userInfo);
        }

        /// <summary>
        /// Şifre değiştir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserInfoDto>> ChangePasswordMobile(ChangePasswordMobileDto model)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            var user = await userRepo.Get(d => d.Email == model.Email && d.PasswordResetOtpCode == model.OtpCode);

            if (user == null)
            {
                return new ServiceResult<UserInfoDto>(null, "Kullanıcı bulunamadı", ResultType.Error);
            }

            var message = EmailTemplateHelper.GenerateChangePasswordMobileSuccessMailContent(user.Name, user.Surname);
            await EmailHelper.SendMail(user.Email, "Şifre Değişimi", message);

            var password = PasswordHelper.HashPassword(model.NewPassword);
            user.Password = password;
            user.PasswordResetGuid = null;
            user.PasswordResetOtpCode = null;
            user.IsPasswordChangeNextLogin = false;
            user.IsActive = true;
            userRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            var userInfo = new UserInfoDto()
            {
                Email = user.Email,
                Fullname = string.Format("{0} {1}", user.Name, user.Surname),
                Name = user.Name,
                Surname = user.Surname,
                Id = user.Id,
                ChangePassword = user.IsPasswordChangeNextLogin,
            };

            return new ServiceResult<UserInfoDto>(userInfo);
        }

        /// <summary>
        /// Profilden şifre değiştir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserInfoDto>> ChangePasswordProfile(ChangePasswordProfileDto model)
        {
            model.UserId = jwtHelper.GetCurrentUser().Id;
            var userRepo = UnitOfWork.GetRepository<User>();
            var user = await userRepo.Get(d => d.Id == model.UserId);

            if (!PasswordHelper.VerifyHashedPassword(user.Password, model.OldPassword))
            {
                return new ServiceResult<UserInfoDto>() { Messages = new List<string>() { "Kullanıcı adı ya da şifre yanlış" }, ResultType = ResultType.Error };
            }

            user.Password = PasswordHelper.HashPassword(model.NewPassword);
            userRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<UserInfoDto>();
        }

        /// <summary>
        /// Şifremi unuttum
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult> ForgetPassword(ForgetPasswordDto model)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            User user = await userRepo.Get(d => d.Email == model.Email && !d.IsDeleted);

            if (user == null)
            {
                return new ServiceResult("Kullanıcı adı ya da şifre yanlış", ResultType.Error);
            }

            var guid = Guid.NewGuid().ToString();
            var message = EmailTemplateHelper.GenerateForgotPasswordMailContent(user.Name, user.Surname, guid);
            await EmailHelper.SendMail(user.Email, "Şifremi Unuttum", message);

            user.PasswordResetGuid = guid;
            user.IsPasswordChangeNextLogin = true;
            //user.Password = "";

            userRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult("Şifre sıfırlama linki eposta adresinize gönderilmiştir");
        }

        /// <summary>
        /// Şifremi unuttum mobil
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult> ForgetPasswordMobile(ForgetPasswordDto model)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            User user = await userRepo.Get(d => d.Email == model.Email && !d.IsDeleted);

            if (user == null)
            {
                return new ServiceResult("Kullanıcı adı ya da şifre yanlış", ResultType.Error);
            }
            Random generator = new Random();
            string otpCode = generator.Next(0, 1000000).ToString("D6");
            var message = EmailTemplateHelper.GenerateForgotPasswordMobileMailContent(user.Name, user.Surname, otpCode);
            await EmailHelper.SendMail(user.Email, "Şifremi Unuttum", message);

            user.PasswordResetOtpCode = otpCode;
            user.IsPasswordChangeNextLogin = true;
            //user.Password = "";

            userRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult("Şifre sıfırlama bilgileri eposta adresinize gönderilmiştir");
        }

        /// <summary>
        /// Kullanıcı guid kontrolü
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> CheckGuid(string guid)
        {
            var user = await UnitOfWork.GetRepository<User>().Get(d => d.PasswordResetGuid == guid && !d.IsDeleted);
            if (user == null)
            {
                return new ServiceResult<bool>(false, "Geçersiz istek", ResultType.Error);
            }
            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Kullanıcı OTP kod kontrolü
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> CheckUserOtpCode(CheckUserOtpDto model)
        {
            var user = await UnitOfWork.GetRepository<User>().Get(d => d.Email == model.Email && d.PasswordResetOtpCode == model.OtpCode && !d.IsDeleted);
            if (user == null)
            {
                return new ServiceResult<bool>(false, "Geçersiz istek", ResultType.Error);
            }

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// OTP kod ile kullanıcı aktifleştirme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> ActivateUserWithOtpCode(CheckUserOtpDto model)
        {
            var user = await UnitOfWork.GetRepository<User>().Get(d => d.Email == model.Email && d.PasswordResetOtpCode == model.OtpCode && !d.IsDeleted, disableTracking: false);
            if (user == null)
            {
                return new ServiceResult<bool>(false, "Geçersiz istek", ResultType.Error);
            }
            if (user.IsActive)
            {
                return new ServiceResult<bool>(false, "Kullanıcı zaten aktifleştirilmiş...", ResultType.Warning);
            }

            user.IsActive = true;
            user.PasswordResetOtpCode = null;
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Başka kullanıcının hesabına geçiş yapma
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserInfoDto>> LoginAnotherUserAccount(string email)
        {
            var currentUser = jwtHelper.GetCurrentUser();
            var user = await UnitOfWork.GetRepository<User>().Get(d => d.Email == email && !d.IsDeleted);
            return await LoginCommon(user, null, currentUser.Id);
        }

        /// <summary>
        /// Profil güncelleme
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateProfile(UserDto userDto)
        {
            var currentUser = jwtHelper.GetCurrentUser();
            userDto.Id = currentUser.Id;

            var userRepo = UnitOfWork.GetRepository<User>();
            User user = await userRepo.Get(d => d.Id == userDto.Id);

            if (user == null)
            {
                return new ServiceResult<long?>(null, "Kullanıcı bulunamadı...", ResultType.Warning);
            }

            user = await userRepo.GetById(userDto.Id.Value);
            user.Name = userDto.Name;
            user.Surname = userDto.Surname;
            user.Phone = userDto.Phone;
            user.ImageUrl = userDto.ImageUrl;
            userRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(user.Id);
        }


        /// <summary>
        /// Kullanıcı üye olma
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> Register(RegisterUserDto registerDto)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            var userRoleRepo = UnitOfWork.GetRepository<UserRole>();
            User user = await userRepo.Get(d => d.Email == registerDto.Email && !d.IsDeleted);

            if (user != null)
            {
                return new ServiceResult<long?>(null, "Email addresi kullanımda", ResultType.Error);
            }

            if (registerDto.Password != registerDto.PasswordConfirm)
            {
                return new ServiceResult<long?>(null, "Girilen şifreler eşleşmiyor", ResultType.Error);
            }

            user = ReflectionHelper.CloneObject<RegisterUserDto, User>(registerDto);
            user.Password = PasswordHelper.HashPassword(registerDto.Password);

            Random generator = new Random();
            user.PasswordResetOtpCode = generator.Next(0, 1000000).ToString("D6");
            user.IsActive = false;

            var result = userRepo.Add(user);
            await UnitOfWork.SaveChangesAsync();

            var message = EmailTemplateHelper.RegisterUserMailContent(registerDto.Name, registerDto.Surname, user.PasswordResetOtpCode);
            await EmailHelper.SendMail(registerDto.Email, "Yeni Üyelik", message);

            #region Roles
            var roleRepo = UnitOfWork.GetRepository<Role>();
            var addingRole = await roleRepo.Get(d => d.Name == RoleNames.User);
            if (addingRole == null)
            {
                return new ServiceResult<long?>(null, "Kayıt sırasında bir problem oluştu. Lütfen sistem yöneticisiyle iletişime geçiniz.", ResultType.Error);
            }
            var addingUserRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = addingRole.Id,
            };

            await userRoleRepo.Add(addingUserRole);
            await UnitOfWork.SaveChangesAsync();
            #endregion

            return new ServiceResult<long?>(user.Id);
        }

        /// <summary>
        /// Edevlet kullanıcısı sistemde ekli değilse kaydını yapar
        /// </summary>
        /// <param name="tckn"></param>
        /// <param name="ad"></param>
        /// <param name="soyad"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserInfoDto>> GetEdevletUser(string tckn, string ad, string soyad)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            var hashedEmail = $"e-devlet-kullanicisi-{CryptoExtensions.EncryptString(Keys.CryptoKey, tckn)}";

            var flatPassword = tckn + ad + soyad;
            var hashedPassword = PasswordHelper.HashPassword(flatPassword);

            User user = await userRepo.Get(d => d.Email == hashedEmail && !d.IsDeleted);

            if (user == null)
            {
                #region Adding user
                user = new User()
                {
                    Name = ad,
                    Surname = soyad,
                    Password = hashedPassword,
                    Email = hashedEmail,
                    IsActive = true
                };

                var result = userRepo.Add(user);
                await UnitOfWork.SaveChangesAsync();
                #endregion

                #region Adding User Role
                var roleRepo = UnitOfWork.GetRepository<Role>();
                var userRoleRepo = UnitOfWork.GetRepository<UserRole>();
                var addingRole = await roleRepo.Get(d => d.Name == RoleNames.User);
                var addingUserRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = addingRole.Id,
                };

                await userRoleRepo.Add(addingUserRole);
                await UnitOfWork.SaveChangesAsync();
                #endregion

                user = await userRepo.Get(d => d.Email == hashedEmail && !d.IsDeleted);
            }

            var loginDto = new LoginDto() { Email = hashedEmail, Password = flatPassword };
            return await LoginCommon(user, loginDto);
        }

        #region privates
        private async Task<ServiceResult<UserInfoDto>> LoginCommon(User user, LoginDto model, long? adminUserId = null)
        {
            if (user == null)
            {
                return new ServiceResult<UserInfoDto>(null, "Kullanıcı adı ya da şifre yanlış.", ResultType.Error);
            }
            if (user.IsActive == false)
            {
                if (!string.IsNullOrEmpty(user.PasswordResetOtpCode))
                {
                    return new ServiceResult<UserInfoDto>(null, "Kullanıcı aktivasyon kodu girilmesi gerekmektedir.", ResultType.Warning);
                }
                else
                {
                    return new ServiceResult<UserInfoDto>(null, "Kullanıcı pasif durumdadır.", ResultType.Error);
                }
            }
            var roleRepo = UnitOfWork.GetRepository<Role>().AsQueryable().Where(d => !d.IsDeleted);
            var userRoleRepo = UnitOfWork.GetRepository<UserRole>().AsQueryable().Where(d => !d.IsDeleted);
            if (user == null)
            {
                return new ServiceResult<UserInfoDto>(null, "Kullanıcı adı ya da şifre yanlış", ResultType.Error);
            }
            if (adminUserId == null)
            {
                if (!PasswordHelper.VerifyHashedPassword(user.Password, model.Password))
                {
                    return new ServiceResult<UserInfoDto>(null, "Kullanıcı adı ya da şifre yanlış", ResultType.Error);
                }
            }

            UserInfoDto userInfo = new UserInfoDto();
            var userInRoles = await UnitOfWork.GetRepository<UserRole>().GetAll(d => d.UserId == user.Id && !d.IsDeleted);
            var roleIds = userInRoles.Select(d => d.RoleId).ToList();
            var roles = await UnitOfWork.GetRepository<Role>().GetAll(d => roleIds.Contains(d.Id));

            userInfo.Email = user.Email;
            userInfo.Fullname = string.Format("{0} {1}", user.Name, user.Surname);
            userInfo.Name = user.Name;
            userInfo.Surname = user.Surname;
            userInfo.Phone = user.Phone;
            userInfo.Id = user.Id;
            userInfo.ImageUrl = user.ImageUrl;
            userInfo.Roles = (from userRole in userRoleRepo
                              join role in roleRepo on userRole.RoleId equals role.Id
                              where userRole.UserId == user.Id && userRole.IsDeleted == false
                              select new UserInfoRoleDto
                              {
                                  Id = userRole.Id,
                                  RoleId = userRole.RoleId,
                                  RoleName = role.Name
                              }).ToList();

            #region UserLogin History
            var userLoginHistory = new UserLoginHistory();
            userLoginHistory.CreatedBy = user.Id;
            userLoginHistory.IpAdress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            userLoginHistory.IsMobile = false;
            userLoginHistory.Browser = "";
            userLoginHistory.AdminUserId = null;
            userLoginHistory.RowGuid = Guid.NewGuid().ToString();
            userLoginHistory.CreatedDate = DateTime.Now;
            userLoginHistory.AdminUserId = adminUserId;
            await UnitOfWork.GetRepository<UserLoginHistory>().Add(userLoginHistory);
            await UnitOfWork.SaveChangesAsync();
            #endregion

            return new ServiceResult<UserInfoDto>(userInfo);
        }
        #endregion
    }
}
