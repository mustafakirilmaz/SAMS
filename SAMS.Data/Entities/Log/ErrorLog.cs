using SAMS.Infrastructure.Entities;
using SAMS.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("error_logs")]
    public class ErrorLog : AuditableEntity
    {
        [StringLength(500)]
        public string MachineName { get; set; }

        public string UserIp { get; set; }

        public LogLevel LogLevel { get; set; }

        [StringLength(4000)]
        public string Message { get; set; }

        [StringLength(4000)]
        public string StackTrace { get; set; }

        public string ErrorCode { get; set; }

        public string Url { get; set; }
    }
}
