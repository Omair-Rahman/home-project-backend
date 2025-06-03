namespace HomeProject.Models.Request.Profile
{
    public class ProfileUpdateDto : ProfileInDto
    {
        public bool IsNew { get; set; } = false;
    }
}
