using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data.Dtos
{
    public class UnitDto
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public long BuildingId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public int Floor { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public double NetSquareMeter { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public double GrossSquareMeter { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public double ShareOfLand { get; set; }
    }
}