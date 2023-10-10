using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("users")]
    public class User : AuditableEntity
    {
        public User()
        {
            UserRoles = new List<UserRole>();
        }

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Surname { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        public string Phone { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        public bool? IsPasswordChangeNextLogin { get; set; }

        public string PasswordResetGuid { get; set; }

        public string PasswordResetOtpCode { get; set; }

        public bool IsActive { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        //Helpers
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public string Role { get; set; }

        [NotMapped]
        public bool IsAdmin { get; set; }
    }
}
