using System.ComponentModel.DataAnnotations.Schema;

namespace HomeProject.Models.Domain
{
    [Table("Profiles")]
    public class Profile : BaseEntity
    {
        public int Rating { get; set; }
        public required string Name { get; set; }
        public string? ProfileUrl { get; set; }
        public string? Image { get; set; }
    }
}
