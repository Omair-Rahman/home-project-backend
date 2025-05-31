using AutoMapper;
using HomeProject.Models.Domain;
using HomeProject.Models.Response.Profile;

namespace HomeProject.ServiceExtensions
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProfileModel, ProfileList>().ReverseMap();
        }
    }
}
