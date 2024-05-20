using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("buildings")]
    public class Building : AuditableEntity
    {
        public bool IsSite { get; set; }
        public long SiteId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        [Required]
        public int CityCode { get; set; }

        [Required]
        public int TownCode { get; set; }

        [Required]
        public int DistrictCode { get; set; }

        public List<Unit> Units { get; set; }
        public List<BusinessProject> BusinessProjects { get; set; }
    }
}
