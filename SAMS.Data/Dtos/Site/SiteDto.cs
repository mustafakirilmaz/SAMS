﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class SiteDto
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Name { get; set; }
    }
}
