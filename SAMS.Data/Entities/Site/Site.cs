using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("sites")]
    public class Site : AuditableEntity
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
    }
}
