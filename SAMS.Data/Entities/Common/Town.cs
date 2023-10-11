using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("Towns", Schema = "common")]
    public class Town : AuditableEntity
    {
        public string Name { get; set; }
        public int CityCode { get; set; }
        public int Code { get; set; }
    }
}
