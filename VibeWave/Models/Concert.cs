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
        [DisplayName("Concert Category")]
        public string ConcertCategory { get; set; }

        [Range(1, 100, ErrorMessage = "Please enter between 1 and 100")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
