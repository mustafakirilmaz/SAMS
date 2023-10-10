using SAMS.Infrastructure.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Infrastructure.Entities
{
    [Table("audits")]
    public class Audit : BaseEntity
    {
        public string TableName { get; set; }

        public long UserId { get; set; }

        public long? RoleId { get; set; }

        public AuditType AuditType { get; set; }

        [MaxLength(3000)]
        public string OldValues { get; set; }

        [MaxLength(3000)]
        public string NewValues { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(36)]
        public string RowGuid { get; set; }
    }
}
