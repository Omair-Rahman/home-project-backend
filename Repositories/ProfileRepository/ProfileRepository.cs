using HomeProject.Repositories.BaseRepository;
using HomeProject.Database;
using HomeProject.Models.Domain;
using HomeProject.Models.Response.Profile;
using Microsoft.EntityFrameworkCore;

namespace HomeProject.Repositories.ProfileRepository
{
    public class ProfileRepository : BaseRepository<ProfileModel>, IProfileRepository
    {
        public ProfileRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<ProfileDetailsResponse?> GetDetailsById(int id)
        {
            var chart = await (from m in _dbContext.MediaContents
                               where m.ProfileId == id
                               group m by m.Rating into mGroup
                               select new MediaRating
                               {
                                   Rating = mGroup.Key,
                                   TotalMediaContents = mGroup.Count(),
                               }).ToListAsync();

            var data = await (from p in _dbContext.Profiles
                              where p.Id == id
                              select new ProfileDetailsResponse
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Image = p.Image,
                                  Rating = p.Rating,
                                  ImageFile = p.ImageFile,
                                  ProfileUrl = p.ProfileUrl,
                                  TotalMediaContents = p.TotalMediaContents,
                              }).FirstAsync();
            
            data.Chart = chart;
            return data;
        }
    }
}
