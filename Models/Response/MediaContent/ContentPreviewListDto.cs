namespace HomeProject.Models.Response.MediaContent
{
    public class ContentPreviewListDto
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty; // e.g., "video", "audio", "image"
        public byte[]? PreviewData { get; set; }
        public bool IsFavourite { get; set; }
    }
}
