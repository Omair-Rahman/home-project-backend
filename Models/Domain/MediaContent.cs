using System.ComponentModel.DataAnnotations.Schema;

namespace HomeProject.Models.Domain
{
    [Table("MediaContents")]
    public class MediaContent : BaseEntity
    {
        public int ProfileId { get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public byte[]? PreviewData { get; set; }
        public byte[]? FullData { get; set; }
    }
}
