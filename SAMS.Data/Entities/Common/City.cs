using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("Cities", Schema = "common")]
    public class City : AuditableEntity
    {
        public string Name { get; set; }
        public int Code { get; set; }
    }
}
