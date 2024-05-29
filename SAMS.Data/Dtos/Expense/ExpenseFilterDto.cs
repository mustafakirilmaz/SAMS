using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class ExpenseFilterDto
    {
        public long? ExpenseTypeId { get; set; }
        public string Name { get; set; }

    }
}