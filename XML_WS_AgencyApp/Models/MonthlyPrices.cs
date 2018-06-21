using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class MonthlyPrices
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? MainServerId { get; set; }

        [Required]
        [Range(typeof(double), "1", "1000000000")]
        public double Amount { get; set; }

        [Required]
        [Range(typeof(int), "1", "12")]
        public int Month { get; set; }

        [Required]
        [Range(typeof(int), "2000", "2100")]
        public int Year { get; set; }

        public BookingUnit BookingUnit { get; set; }
    }
}