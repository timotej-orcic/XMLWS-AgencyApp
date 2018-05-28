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

        public string Address { get; set; }
        public string Description { get; set; }
        public int PeopleNo { get; set; }
        public List<BookingUnitPicture> MyPictures { get; set; }
        public List<AccomodationCategory> MyAccomodationCategories { get; set; }
        public List<AccomodationType> MyAccomodationTypes { get; set; }
        public List<BonusFeatures> MyBonusFeatures { get; set; }
        public List<MonthlyPrices> MyMonthlyPrices { get; set; }
        public List<Reservation> MyReservations { get; set; }
    }
}