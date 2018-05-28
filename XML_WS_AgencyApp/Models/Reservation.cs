using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal Price { get; set; }
        public bool Confirmed { get; set; }
        public short Rating { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}