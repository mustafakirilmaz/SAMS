using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("Cities", Schema = "Common")]
    public class City : AuditableEntity
    {
        public string Name { get; set; }
        public int Code { get; set; }
    }
}
