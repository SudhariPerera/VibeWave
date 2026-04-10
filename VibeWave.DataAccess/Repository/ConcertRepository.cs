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
    public class ConcertRepository : Repository<Concert>, IConcertRepository
    {
        private ApplicationDbContext _db;
        public ConcertRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Concert obj)
        {
            _db.Update(obj);
        }
    }
}
