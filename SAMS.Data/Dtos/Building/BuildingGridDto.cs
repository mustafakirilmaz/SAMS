using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class BuildingGridDto
    {
        public long Id { get; set; }        
        public string SiteName { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string District { get; set; }

    }
}
