using System.ComponentModel.DataAnnotations.Schema;

namespace HomeProject.Models.Domain
{
    [Table("Profiles")]
    public class ProfileModel : BaseEntity
    {
        public int Rating { get; set; }
        public int TotalMediaContents { get; set; }
        public required string Name { get; set; }
        public string? ProfileUrl { get; set; }
        public string? Image { get; set; }
        public byte[]? ImageFile { get; set; }
    }
}
