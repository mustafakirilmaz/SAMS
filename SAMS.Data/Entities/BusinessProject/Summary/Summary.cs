using SAMS.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("summary")]
    public class Summary : AuditableEntity
    {
        public long? BusinessProjectId { get; set; }

        public List<EqualExpense> EqualExpenses { get; set; }
        public List<ProportionalExpense> PerproportionalExpenses { get; set; }
        public List<FixtureExpense> FixtureExpenses { get; set; }
        public List<Unit> Units { get; set; }

    }
}