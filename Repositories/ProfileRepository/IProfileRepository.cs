using HomeProject.Models.Domain;
using HomeProject.Models.Response.Profile;
using HomeProject.Repositories.BaseRepository;

namespace HomeProject.Repositories.ProfileRepository
{
    public interface IProfileRepository : IBaseRepository<ProfileModel>
    {
        Task<ProfileDetailsResponse?> GetDetailsById(int id);
    }
}
