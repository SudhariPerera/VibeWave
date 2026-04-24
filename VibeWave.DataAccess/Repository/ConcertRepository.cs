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
            var objFromDb = _db.Concert.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.ConcertName = obj.ConcertName;
                objFromDb.ActorName = obj.ActorName;
                objFromDb.Category = obj.Category;
                objFromDb.ConcertLocation = obj.ConcertLocation;
                objFromDb.DisplayDate = obj.DisplayDate;
                objFromDb.DisplayTime = obj.DisplayTime;
                if(obj.ConcertImageUrl != null)
                {
                    objFromDb.ConcertImageUrl = obj.ConcertImageUrl;
                }
            }
        }
    }
}
