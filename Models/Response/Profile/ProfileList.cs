namespace HomeProject.Models.Response.Profile
{
    public class ProfileList
    {
        public int Rating { get; set; }
        public required string Name { get; set; }
        public string? ProfileUrl { get; set; }
        public string? Image { get; set; }
        public byte[]? ImageFile { get; set; }
    }
}
