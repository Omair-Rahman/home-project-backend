using HomeProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeProject.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : base(dbContext) { }

        public DbSet<ProfileModel> Profiles { get; set; }
        public DbSet<MediaContent> MediaContents { get; set; }
    }
}
