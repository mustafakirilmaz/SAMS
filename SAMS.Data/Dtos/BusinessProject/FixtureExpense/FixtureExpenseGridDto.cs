using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class FixtureExpenseGridDto
    {
        public long Id { get; set; }
        public string FixtureExpenseType { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }

    }
}
