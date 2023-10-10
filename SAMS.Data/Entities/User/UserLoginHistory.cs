using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("user_login_history")]
    public class UserLoginHistory : AuditableEntity
    {
        [Required, MaxLength(32)]
        public string IpAdress { get; set; }

        [MaxLength(50)]
        public long? AdminUserId { get; set; }

        [Required]
        public bool IsMobile { get; set; }

        [MaxLength(256)]
        public string Browser { get; set; }
    }
}
