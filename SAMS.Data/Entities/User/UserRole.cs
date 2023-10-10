using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("user_role")]
    public class UserRole : AuditableEntity
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
