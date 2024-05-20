using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class UnitGridDto
    {
        public long Id { get; set; }
        public string SiteName { get; set; }
        public string BuildingName { get; set; }
        public string UnitName { get; set; }
        public string NetSquareMeter { get; set; }
        public string GrossSquareMeter { get; set; }
        public string ShareOfLand { get; set; }

    }
}
