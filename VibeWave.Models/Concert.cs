using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VibeWave.Models
{
    public class Concert
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Concert Name")]
        public string ConcertName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Actor Name")]
        public string ActorName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Concert Category")]
        public string ConcertCategory { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Concert Location")]
        public string ConcertLocation { get; set; }
        

        [Required]
        [DisplayName("Display Date")]
        [DataType(DataType.Date)]
        public DateOnly DisplayDate { get; set; }

        [Required]
        [DisplayName("Display Time")]
        [DataType(DataType.Time)]
        public TimeOnly DisplayTime { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Ticket Price")]
        public string TicketPrice { get; set; }
    }
}
