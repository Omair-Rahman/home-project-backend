namespace HomeProject.Models.Response.MediaContent
{
    public class ContentDetailsDto
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty; // e.g., "video", "audio", "image"
        public string? FullPath { get; set; }
        public byte[]? FullData { get; set; }
        public bool IsFavourite { get; set; }
    }
}
