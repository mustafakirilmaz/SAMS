using SAMS.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data.Dtos
{
    public class EqualExpenseDto
    {
        public long? Id { get; set; }

        //[Required(ErrorMessage = "{0} boş geçilemez.")]
        public long? BusinessProjectId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public EqualExpenseTypesEnum EqualExpenseType { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public double Cost { get; set; }
    }
}