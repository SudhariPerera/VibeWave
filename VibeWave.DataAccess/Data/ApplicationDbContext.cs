using Microsoft.EntityFrameworkCore;
using VibeWave.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VibeWave.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Concert> Concert { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//AI1. 必须首先调用基类方法，让 Identity 完成自己的配置（如主键等）[reference:2]
            modelBuilder.Entity<Concert>().HasData(
                new Concert
                {
                    Id = 1,
                    ConcertName = "Come Together - Born to Run - Bruce Springsteen",
                    ActorName = "ABC",
                    ConcertLocation = "A",
                    DisplayDate = new DateOnly(2026, 05, 01),
                    DisplayTime = new TimeOnly(00, 20),
                    TicketPrice = 50,
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
                    TicketPrice = 50,
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
                    TicketPrice = 50,
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