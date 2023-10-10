using System;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Infrastructure.Entities
{
    public class AuditableEntity : BaseEntity
    {
        public long? CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [MaxLength(36)]
        public string RowGuid { get; set; }
    }
}
