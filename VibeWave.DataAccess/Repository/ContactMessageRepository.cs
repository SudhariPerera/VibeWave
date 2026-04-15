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
    public class ContactMessageRepository : Repository<ContactMessage>, IContactMessageRepository
    {
        private readonly ApplicationDbContext _db;
        public ContactMessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ContactMessage obj)
        {
            _db.ContactMessages.Update(obj);
        }
    }
}
