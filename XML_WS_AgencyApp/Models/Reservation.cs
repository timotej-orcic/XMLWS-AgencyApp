using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public enum ReservationStatus
    {
        WAITING, CONFIRMED, CANCELED
    }

    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? MainServerId { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public ReservationStatus ReservationStatus { get; set; }

        [MaxLength(60)]
        public string SubjectName { get; set; }

        [MaxLength(60)]
        public string SubjectSurname { get; set; }

        public BookingUnit BookingUnit { get; set; }
    }
}