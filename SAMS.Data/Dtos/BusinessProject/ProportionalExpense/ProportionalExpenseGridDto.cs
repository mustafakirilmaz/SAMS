using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class ProportionalExpenseGridDto
    {
        public long Id { get; set; }
        public string ProportionalExpenseType { get; set; }
        public double Cost { get; set; }

    }
}
