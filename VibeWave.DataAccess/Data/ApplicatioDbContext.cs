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
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concert>().HasData(
                new Concert
                {
                    Id = 1,
                    ConcertName = "Come Together - Born to Run - Bruce Springsteen",
                    ActorName = "ABC",
                    ConcertLocation = "A",
                    DisplayDate = new DateOnly(2026, 05, 01),
                    DisplayTime = new TimeOnly(00, 20),
                    TicketPrice = "50",
                    CategoryId = 1
                },
                new Concert
                {
                    Id = 2,
                    ConcertName = "Kyla Cobbler - Not My Lemons",
                    ActorName = "ABC",
                    ConcertLocation = "A",
                    DisplayDate = new DateOnly(2026, 05, 01),
                    DisplayTime = new TimeOnly(00, 20),
                    TicketPrice = "50",
                    CategoryId = 2
                },
                new Concert
                {
                    Id = 3,
                    ConcertName = "Nurse Georgie Carroll - Infectious",
                    ActorName = "ABC",
                    ConcertLocation = "A",
                    DisplayDate = new DateOnly(2026, 05, 01),
                    DisplayTime = new TimeOnly(00, 20),
                    TicketPrice = "50",
                    CategoryId = 3
                }
                );
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Action",
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Sci-Fi",
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "History",
                }
                );
        }
    }
}