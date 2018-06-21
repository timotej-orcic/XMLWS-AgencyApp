using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class BookingUnit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? MainServerId { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        [MaxLength(90)]
        public string Address { get; set; }

        public City City { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(int), "1", "2147483647")]
        public int PeopleNo { get; set; }

        public ApplicationUser Agent { get; set; }

        public AccomodationType AccomodationType { get; set; }

        public AccomodationCategory AccomodationCategory { get; set; }

        public ICollection<BookingUnitPicture> Pictures { get; set; }

        public ICollection<BonusFeatures> BonusFeatures { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}