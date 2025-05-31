namespace HomeProject.Models.Request.Profile
{
    public class ProfileFilter : FilterBase
    {
        public string? Name { get; set; }
        public int? Rating { get; set; }
    }
}
