using HomeProject.Models.Domain;
using HomeProject.Models.Request.Profile;
using HomeProject.Models.Response;
using HomeProject.Models.Response.Profile;

namespace HomeProject.Services.ProfileService
{
    public interface IProfileService
    {
        Task<PaginatedResponseModel<ProfileModel>> Get(ProfileFilter request);
        Task<ResponseModel<ProfileModel?>> GetById(int id);
        Task<ResponseModel<ProfileDetailsResponse?>> GetDetailsById(int id);
        Task<ResponseModel<object>> Create(ProfileInDto request);
        Task<ResponseModel<object>> Update(int id, ProfileUpdateDto request);
        Task<ResponseModel<object>> Delete(int id);
    }
}
