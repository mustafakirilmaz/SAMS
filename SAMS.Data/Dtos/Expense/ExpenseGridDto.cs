using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class ExpenseGridDto
    {
        public long Id { get; set; }        
        public string ExpenseTypeName { get; set; }
        public string Name { get; set; }

    }
}
