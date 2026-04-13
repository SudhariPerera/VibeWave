using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeWave.Data;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

namespace VibeWave.DataAccess.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private ApplicationDbContext _db;
        public BookingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Booking obj)
        {
            _db.Update(obj);
        }
    }
}
