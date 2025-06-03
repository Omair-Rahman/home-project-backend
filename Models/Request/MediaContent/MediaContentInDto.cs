namespace HomeProject.Models.Request.MediaContent
{
    public class MediaContentInDto
    {
        public int ProfileId { get; set; }
        public IFormFile? MediaFile { get; set; }
    }
}
