using HomeProject.Repositories.BaseRepository;
using HomeProject.Database;
using HomeProject.Models.Domain;

namespace HomeProject.Repositories.ProfileRepository
{
    public class ProfileRepository : BaseRepository<ProfileModel>, IProfileRepository
    {
        public ProfileRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
