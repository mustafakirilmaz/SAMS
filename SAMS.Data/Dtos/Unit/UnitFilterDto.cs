using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class UnitFilterDto
    {
        public long? Id { get; set; }
        public long? SiteId { get; set; }
        public long? BuildingId { get; set; }
        public string Name { get; set; }

    }
}
