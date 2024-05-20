using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class EqualExpenseGridDto
    {
        public long Id { get; set; }
        public string EqualExpenseType { get; set; }
        public double Cost { get; set; }

    }
}
