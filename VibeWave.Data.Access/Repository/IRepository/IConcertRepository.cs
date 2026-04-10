using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeWave.Models;

namespace VibeWave.DataAccess.Repository.IRepository
{
    public interface IConcertRepository : IRepository<Concert>
    {
        void Update(Concert obj);
    }
}
