using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("roles")]
    public class Role : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
