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
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private ApplicationDbContext _db;

        public PaymentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Payment obj)
        {
            _db.Payments.Update(obj);
        }
    }
}
