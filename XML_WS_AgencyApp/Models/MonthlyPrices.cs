using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class MonthlyPrices
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Amount { get; set; }

        [Required]
        [Range(typeof(int), "1", "12")]
        public int Month { get; set; }

        [Required]
        [Range(typeof(int), "2000", "2100")]
        public int Year { get; set; }

        public BookingUnit BookingUnit { get; set; }
    }
}