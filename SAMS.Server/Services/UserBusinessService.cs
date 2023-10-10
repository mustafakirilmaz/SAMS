using SAMS.Common.Extensions;
using SAMS.Common.Helpers;
using SAMS.Data;
using SAMS.Data.Dtos;
using SAMS.DataAccess;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.Models;
using SAMS.Infrastructure.WebToken;
using SAMS.Server.ServiceContracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAMS.Server.Services
{
    /// <summary>
    /// Kullanıcı işlemleri servisi
    /// </summary>
    public class UserBusinessService : IUserBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Kullanıcı işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public UserBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Kullanıcı ekle
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddUser(UserDto userDto)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            var userRoleRepo = UnitOfWork.GetRepository<UserRole>();
            User user = await userRepo.Get(d => d.Email == userDto.Email && !d.IsDeleted);

            if (user != null)
            {
                return new ServiceResult<long?>(null, "Email addresi kullanımda", ResultType.Error);
            }

            var guid = Guid.NewGuid().ToString();
            var message = EmailTemplateHelper.GenerateCreateUserMailContent(userDto.Name, userDto.Surname, guid);
            await EmailHelper.SendMail(userDto.Email, "Kullanıcı Kaydı", message);

            user = ReflectionHelper.CloneObject<UserDto, User>(userDto);
            user.PasswordResetGuid = guid;
            user.IsPasswordChangeNextLogin = true;
            user.Password = "";

            var result = userRepo.Add(user);
            await UnitOfWork.SaveChangesAsync();

            #region Roles
            if (userDto.RoleIds != null)
            {
                foreach (var roleId in userDto.RoleIds)
                {
                    var addingUserRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                    };

                    await userRoleRepo.Add(addingUserRole);
                }
                await UnitOfWork.SaveChangesAsync();
            }
            #endregion

            return new ServiceResult<long?>(user.Id);
        }

        /// <summary>
        /// Kullanıcı güncelle
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateUser(UserDto userDto)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            var userRoleRepo = UnitOfWork.GetRepository<UserRole>();
            User user = await userRepo.Get(d => d.Email == userDto.Email && d.Id == userDto.Id);

            if (user == null)
            {
                return new ServiceResult<long?>(null, "Kullanıcı bulunamadı", ResultType.Warning);
            }

            user = await userRepo.GetById(userDto.Id.Value);
            user = user.CopyObject(userDto, "Password");
            userRepo.Update(user);
            await UnitOfWork.SaveChangesAsync();

            #region Roles
            var userRoleList = await userRoleRepo.GetAll(d => d.UserId == user.Id && !d.IsDeleted);
            if (userRoleList.Count() > 0)
            {
                userRoleRepo.DeleteAll(userRoleList, true);
            }
            if (userDto.RoleIds != null)
            {
                foreach (var roleId in userDto.RoleIds)
                {
                    var addingUserRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                    };

                    await userRoleRepo.Add(addingUserRole);
                }
                await UnitOfWork.SaveChangesAsync();
            }
            #endregion

            return new ServiceResult<long?>(user.Id);
        }

        /// <summary>
        /// Kullanıcı silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteUser(long id)
        {
            var userRepo = UnitOfWork.GetRepository<User>();
            var userRoleRepo = UnitOfWork.GetRepository<UserRole>();
            var userRoles = await userRoleRepo.GetAll(d => !d.IsDeleted && d.UserId == id);
            userRoleRepo.DeleteAll(userRoles);
            userRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre kullanıcı getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserDto>> GetUserById(long id)
        {
            var user = await UnitOfWork.GetRepository<User>().GetById(id);
            var roleRepo = UnitOfWork.GetRepository<Role>().AsQueryable();
            var userRoleRepo = UnitOfWork.GetRepository<UserRole>().AsQueryable();

            var userDto = ReflectionHelper.CloneObject<User, UserDto>(user);
            userDto.RoleIds = UnitOfWork.GetRepository<UserRole>().GetAll(d => d.UserId == user.Id && !d.IsDeleted).Result.Select(d => d.RoleId).ToList();

            return new ServiceResult<UserDto>(userDto);
        }

        /// <summary>
        /// Sisteme login olan kullanıcının kendi bilgilerini getirir(Gelen token bilgisine göre kullanıcının bilgilerini döner)
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<UserDto>> GetCurrentUser()
        {
            var currentUser = jwtHelper.GetCurrentUser();
            var user = await UnitOfWork.GetRepository<User>().GetById(currentUser.Id);
            var roleRepo = UnitOfWork.GetRepository<Role>().AsQueryable();
            var userRoleRepo = UnitOfWork.GetRepository<UserRole>().AsQueryable();

            var userDto = ReflectionHelper.CloneObject<User, UserDto>(user);
            userDto.RoleIds = UnitOfWork.GetRepository<UserRole>().GetAll(d => d.UserId == user.Id && !d.IsDeleted).Result.Select(d => d.RoleId).ToList();

            return new ServiceResult<UserDto>(userDto);
        }

        /// <summary>
        /// Kullanıcı arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<UserGridDto>>> SearchUsers(QueryFilter<UserFilterDto> queryFilter)
        {
            var userRepo = UnitOfWork.GetRepository<User>().AsQueryable();
            if (!string.IsNullOrEmpty(queryFilter.SearchFilter.Name))
            {
                userRepo = userRepo.Where(d => d.Name.ToLower().Contains(queryFilter.SearchFilter.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(queryFilter.SearchFilter.Surname))
            {
                userRepo = userRepo.Where(d => d.Surname.ToLower().Contains(queryFilter.SearchFilter.Surname.ToLower()));
            }
            var query = from user in userRepo
                        where user.IsDeleted != true
                        select new UserGridDto()
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Surname = user.Surname,
                            Email = user.Email,
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}