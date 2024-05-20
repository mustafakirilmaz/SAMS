using SAMS.Infrastructure.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("fixture_expense")]
    public class FixtureExpense : AuditableEntity
    {
        public long? BusinessProjectId { get; set; }

        [Required]
        public FixtureExpenseTypesEnum FixtureExpenseType { get; set; }

        public string Description { get; set; }

        [Required]
        public double Cost { get; set; }
    }
}