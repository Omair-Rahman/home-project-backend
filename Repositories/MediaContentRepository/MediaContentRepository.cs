using HomeProject.Repositories.BaseRepository;
using HomeProject.Database;
using HomeProject.Models.Domain;

namespace HomeProject.Repositories.MediaContentRepository
{
    public class MediaContentRepository : BaseRepository<MediaContent>, IMediaContentRepository
    {
        public MediaContentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
