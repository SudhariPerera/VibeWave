using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeWave.Models.ViewModels
{
    public class HomeConcertVM
    {
        public int Id { get; set; }
        public string ConcertName { get; set; }
        public string ActorName { get; set; }
        public string ConcertLocation { get; set; }
        public DateOnly DisplayDate { get; set; }
        public string DisplayTime { get; set; }
        public decimal TicketPrice { get; set; }
        public string CategoryName { get; set; }
        public string? ConcertImageUrl { get; set; }
        public bool IsBookable { get; set; }
    }
}
