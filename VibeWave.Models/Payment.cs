using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeWave.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        public string PaymentIntentId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}