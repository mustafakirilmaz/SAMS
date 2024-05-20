using SAMS.Infrastructure.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("proportional_expense")]
    public class ProportionalExpense : AuditableEntity
    {
        public long? BusinessProjectId { get; set; }

        [Required]
        public ProportionalExpenseTypesEnum ProportionalExpenseType { get; set; }

        [Required]
        public double Cost { get; set; }
    }
}