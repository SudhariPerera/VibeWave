using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeWave.DataAccess.Repository.IRepository;

namespace VibeWave.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IConcertRepository Concert { get; }
        IBookingRepository Booking { get; }
        ICategoryRepository Category { get; }
        IContactMessageRepository ContactMessage { get; }
        void Save();
    }
}
