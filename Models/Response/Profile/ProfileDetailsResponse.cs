namespace HomeProject.Models.Response.Profile
{
    public class ProfileDetailsResponse
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public int TotalMediaContents { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfileUrl { get; set; }
        public string? Image { get; set; }
        public byte[]? ImageFile { get; set; }
        public List<MediaRating>? Chart { get; set; }
    }

    public class MediaRating
    {
        public int Rating { get; set; }
        public int TotalMediaContents { get; set; }
    }
}
