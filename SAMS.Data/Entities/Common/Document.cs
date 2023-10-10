using SAMS.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Data
{
    [Table("documents")]
    public class Document : AuditableEntity
    {
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string DownloadLink { get; set; }
        public string FolderName { get; set; }
        public string ReferenceGuid { get; set; }
        public string Url { get; set; }
    }
}
