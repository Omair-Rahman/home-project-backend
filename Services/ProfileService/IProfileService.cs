using Foodi.UserServiceProject.Models.ResponseModels;
using HomeProject.Models.Domain;
using HomeProject.Models.Request.Profile;

namespace HomeProject.Services.ProfileService
{
    public interface IProfileService
    {
        Task<PaginatedResponseModel<ProfileModel>> Get(ProfileFilter request);
        Task<ResponseModel<ProfileModel?>> GetById(int id);
        Task<ResponseModel<object>> Create(ProfileInDto request);
        Task<ResponseModel<object>> Update(int id, ProfileUpdateDto request);
        Task<ResponseModel<object>> Delete(int id);
    }
}
