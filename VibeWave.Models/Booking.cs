using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeWave.Models
{
    public class Booking
        {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ConcertId { get; set; }

        [ForeignKey("ConcertId")]
        public Concert? Concert { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, 10)]
        [Display(Name = "Number of Tickets")]
        public int NumberOfTickets { get; set; }

        public DateTime BookingDate { get; set; }
    }
}
