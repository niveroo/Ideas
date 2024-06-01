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

        [Range(1, 5)]
        public int? Points1 { get; set; }
        [Range(1, 5)]
        public int? Points2 { get; set; }
        [Range(1, 5)]
        public int? Points3 { get; set; }

        public string? Ans1 { get; set; }
        public string? Ans2 { get; set; }
        public string? Ans3 { get; set; }

        public DateTime? ReadyFor2 { get; set; }
        public DateTime? ReadyFor3 { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
    }
}
