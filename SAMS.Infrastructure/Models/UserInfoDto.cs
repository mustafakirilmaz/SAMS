using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Infrastructure.Models
{
    public class UserInfoDto
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fullname { get; set; }
        public DateTime LoginDate { get; set; }
        public string Token { get; set; }
        public bool? ChangePassword { get; set; }
        public string Phone { get; set; }
        public string UserIp { get; set; }
        public string ImageUrl { get; set; }
        public List<UserInfoRoleDto> Roles { get; set; }
        public UserInfoRoleDto? SelectedRole { get; set; }
    }

    public class UserInfoRoleDto
    {
        [Required(ErrorMessage ="{0} boş geçilemez.")]
        public long? Id { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public long? RoleId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string RoleName { get; set; }
    }
}
