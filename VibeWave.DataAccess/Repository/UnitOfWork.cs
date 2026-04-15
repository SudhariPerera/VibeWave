using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeWave.Data;
using VibeWave.DataAccess.Repository.IRepository;

namespace VibeWave.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IConcertRepository Concert { get; private set; }
        public IBookingRepository Booking { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IContactMessageRepository ContactMessage { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Concert = new ConcertRepository(_db);
            Booking = new BookingRepository(_db);
            Category = new CategoryRepository(_db);
            ContactMessage = new ContactMessageRepository(_db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
