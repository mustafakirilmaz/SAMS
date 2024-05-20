using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data.Dtos
{
    public class BuildingDto
    {
        public long? Id { get; set; }

        public long? SiteId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public int CityCode { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public int TownCode { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public int DistrictCode { get; set; }
    }
}