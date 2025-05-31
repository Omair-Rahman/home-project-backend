using HomeProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeProject.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : base(dbContext) { }

        public DbSet<Profile> Profiles { get; set; }
    }
}
