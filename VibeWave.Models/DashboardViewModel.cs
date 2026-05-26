using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeWave.Models
{
    public class DashboardViewModel
    {
        public int TotalConcerts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalBookings { get; set; }
        public int TotalUsers { get; set; }
        public int TotalMessages { get; set; }

        public decimal TotalRevenue { get; set; }
        public int PaidBookings { get; set; }
        public int PendingBookings { get; set; }

        public int RefundedBookings { get; set; }
    }
}
