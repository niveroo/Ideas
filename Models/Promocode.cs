using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ideas.Models
{
    [Table("Promocodes")]
    public class Promocode
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        public DateTime Expires { get; set; }

        public string Discount { get; set; }
    }
}
