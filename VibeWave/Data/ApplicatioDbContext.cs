using Microsoft.EntityFrameworkCore;
using VibeWave.Models;

namespace VibeWave.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Concert> Concert { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concert>().HasData(
                new Concert { Id = 1, ConcertName = "Come Together - Born to Run - Bruce Springsteen", ActorName = "ABC", ConcertCategory = "More Music", DisplayDate = new DateOnly(2026, 05, 01), DisplayTime = new TimeOnly(00, 20) },
                new Concert { Id = 2, ConcertName = "Kyla Cobbler - Not My Lemons", ActorName = "ABC", ConcertCategory = "Comedy", DisplayDate = new DateOnly(2026, 05, 01), DisplayTime = new TimeOnly(00, 20) },
                new Concert { Id = 3, ConcertName = "Nurse Georgie Carroll - Infectious", ActorName = "ABC", ConcertCategory = "Rock and Pop", DisplayDate = new DateOnly(2026, 05, 01), DisplayTime = new TimeOnly(00, 20) }
                );
        }
    }
}
