using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class BuildingFilterDto
    {
        //public long? Id { get; set; }
        public string Name { get; set; }
        public int? CityCode { get; set; }
        public int? TownCode { get; set; }
        public int? DistrictCode { get; set; }

    }
}