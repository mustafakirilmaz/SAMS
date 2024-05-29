using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("expenses")]
    public class Expense : AuditableEntity
    {
        public long ExpenseTypeId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
    }
}
