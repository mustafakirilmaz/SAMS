using SAMS.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data.Dtos
{
    public class ProportionalExpenseDto
    {
        public long? Id { get; set; }

        //[Required(ErrorMessage = "{0} boş geçilemez.")]
        public long? BusinessProjectId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public ProportionalExpenseTypesEnum ProportionalExpenseType { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public double Cost { get; set; }
    }
}