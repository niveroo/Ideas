using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ideas.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public int Points { get; set; }

        public string Text { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
    }
}
