using SAMS.Infrastructure.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("expense_types")]
    public class ExpenseType : AuditableEntity
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
    }
}
