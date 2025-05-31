namespace HomeProject.Models.Request.Profile
{
    public class ProfileInDto
    {
        public int Rating { get; set; }
        public required string Name { get; set; }
        public string? ProfileUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
