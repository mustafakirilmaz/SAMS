using SAMS.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data.Dtos
{
    public class FixtureExpenseDto
    {
        public long? Id { get; set; }

        //[Required(ErrorMessage = "{0} boş geçilemez.")]
        public long? BusinessProjectId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public FixtureExpenseTypesEnum FixtureExpenseType { get; set;}

        public string Description { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public double Cost { get; set; }
    }
}