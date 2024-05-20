using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("units")]
    public class Unit : AuditableEntity
    {
        public long BuildingId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required]
        public double NetSquareMeter { get; set; }

        [Required]
        public double GrossSquareMeter { get; set; }

        [Required]
        public double ShareOfLand { get; set; }
    }
}
