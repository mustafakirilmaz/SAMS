using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data.Dtos
{
    public class ExpenseDto
    {
        public long? Id { get; set; }

        public long? ExpenseTypeId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Name { get; set; }
    }
}