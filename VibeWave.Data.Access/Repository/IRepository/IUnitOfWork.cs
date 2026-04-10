using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeWave.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IConcertRepository Concert {  get; }
        void Save();
    }
}
