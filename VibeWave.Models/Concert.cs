using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeWave.Models;

namespace VibeWave.Models
{
    public class Concert
    {
        [Key]
        public int Id { get; set; }// 定义公共属性 Id，类型为 int，用于存储演唱会的唯一标识符

        [Required]// [Required] 特性：表示 ConcertName 字段在数据库中不能为 NULL，表单验证时必须提供值
        [MaxLength(100)]
        [DisplayName("Concert Name")]
        public string ConcertName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Actor Name")]
        public string ActorName { get; set; }

        //[Required]
        //[MaxLength(100)]
        //[DisplayName("Concert Category")]
        //public string ConcertCategory { get; set; }
        //Cancel ConcertCategory filed，change to use CategoryId to connect foreign Key and Category table

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
        [DisplayName("Ticket Price")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TicketPrice { get; set; }

        //foreign Key
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        //定义了一个整数类型的属性 CategoryId，它会被映射为 Concert 表中的一个列。这个列的值是 Category 表的主键值

        //navigation property
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        //这是一个 导航属性。它不会直接映射为数据库列，而是允许你在代码中通过 Concert 对象直接访问它所属的 Category 对象。例如：concert.Category.Name 可以获取到分类名称。
    }
}