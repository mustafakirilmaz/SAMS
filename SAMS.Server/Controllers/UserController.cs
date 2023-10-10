using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAMS.Data.Dtos;
using SAMS.Infrastructure.Attributes;
using SAMS.Infrastructure.Constants;
using SAMS.Infrastructure.Controller;
using SAMS.Infrastructure.Models;
using SAMS.Server.ServiceContracts;

namespace SAMS.Server.Controllers
{
    /// <summary>
    /// Kullanıcı işlemleri
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IUserBusinessService userService;

        /// <summary>
        /// Kullanıcı işlemleri ctor
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserBusinessService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Id'ye göre kullanıcı getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<ServiceResult<UserDto>> GetUserById(long id)
        {
            return await userService.GetUserById(id);
        }

        /// <summary>
        /// Kullanıcı ekleme
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpPost]
        public async Task<ServiceResult<long?>> CreateUser([FromBody] UserDto userDto)
        {
            return await userService.AddUser(userDto);
        }

        /// <summary>
        /// Kullanıcı güncelleme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Roles(RoleNames.User, RoleNames.Admin)]
        [HttpPut("{id}")]
        public async Task<ServiceResult<long?>> UpdateUser(long id, [FromBody] UserDto userDto)
        {
            userDto.Id = id;
            return await userService.UpdateUser(userDto);
        }

        /// <summary>
        /// Kayıtlı kullanıcıları arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpGet("search")]
        public async Task<ServiceResult<List<UserGridDto>>> SearchUsers([FromQuery] QueryFilter<UserFilterDto> queryFilter)
        {
            return await userService.SearchUsers(queryFilter);
        }

        /// <summary>
        /// Kullanıcı silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Roles(RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<ServiceResult<bool>> DeleteUser(long id)
        {
            return await userService.DeleteUser(id);
        }

        /// <summary>
        /// Sisteme login olan kullanıcının kendi bilgilerini getirir(Gelen token bilgisine göre kullanıcının bilgilerini döner)
        /// </summary>
        /// <returns></returns>
        [HttpGet("current")]
        public async Task<ServiceResult<UserDto>> GetCurrentUser()
        {
            return await userService.GetCurrentUser();
        }
    }
}
