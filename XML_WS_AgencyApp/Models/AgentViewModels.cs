using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XML_WS_AgencyApp.Models
{
    public class AddNewBookingUnitViewModel
    {
        [Required(ErrorMessage = "The adress field is required")]
        [MaxLength(90, ErrorMessage = "Max length is 90 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The accomodation type is required")]
        public AccomodationType AccomodationType { get; set; }

        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "At least one image is required")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Images { get; set; }

        [Required(ErrorMessage = "Unit capacity filed is required")]
        [Range(typeof(int), "1", "2147483647", ErrorMessage = "The minimum unit capacity is 1 person")]
        public int PeopleNo { get; set; }

        public List<BonusFeatures> BonusFeatures { get; set; }

        public List<MonthlyPrices> MonthlyPrices { get; set; }
    }
}