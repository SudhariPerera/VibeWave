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

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Concert = new ConcertRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
