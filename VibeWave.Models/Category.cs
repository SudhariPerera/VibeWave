using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace VibeWave.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Category Name")]//[DisplayName("Category Name")] 特性：在视图中显示标签时，使用 "Category Name" 而不是默认的 "Name"
        public string Name { get; set; }

    }
}
