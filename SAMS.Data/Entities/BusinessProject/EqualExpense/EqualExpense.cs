using SAMS.Infrastructure.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("equal_expense")]
    public class EqualExpense : AuditableEntity
    {
        public long? BusinessProjectId { get; set; }

        [Required]
        public EqualExpenseTypesEnum EqualExpenseType { get; set; }

        [Required]
        public double Cost { get; set; }
    }
}
