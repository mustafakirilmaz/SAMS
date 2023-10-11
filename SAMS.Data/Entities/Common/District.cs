using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("Districts", Schema = "common")]
    public class District : AuditableEntity
    {
        public string Name { get; set; }
        public int CityCode { get; set; }
        public int TownCode { get; set; }
        public int Code { get; set; }
    }
}
