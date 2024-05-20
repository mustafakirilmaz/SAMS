using SAMS.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data.Dtos
{
    public class BusinessProjectDto
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")] 
        public long BuildingId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")] 
        public DateTime EndDate { get; set; }
    }
}