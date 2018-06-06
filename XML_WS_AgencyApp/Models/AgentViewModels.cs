using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XML_WS_AgencyApp.Models
{
    public class AddNewBookingUnitViewModel
    {
        [Required(ErrorMessage = "The adress field is required")]
        [MaxLength(90, ErrorMessage = "Max length is 90 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The country is required")]
        public string CountryId { get; set; }
        public IEnumerable<SelectListItem> CountriesList { get; set; }

        [Required(ErrorMessage = "The city is required")]
        public string CityId { get; set; }
        public IEnumerable<SelectListItem> CitiesList { get; set; }

        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Guest capacity filed is required")]
        [Range(typeof(int), "1", "2147483647", ErrorMessage = "The minimum unit capacity is 1 person")]
        [RegularExpression(@"^([1-9]\d*|0)$", ErrorMessage = "Must be an integer")]
        public int PeopleNo { get; set; }

        [Required(ErrorMessage = "The accomodation type is required")]
        public string AccomodationTypeId { get; set; }
        public IEnumerable<SelectListItem> AccomodationTypesList { get; set; }

        [Required(ErrorMessage = "At least one image is required")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Images { get; set; }

        public List<BonusFeatures> BonusFeatures { get; set; }

        public List<MonthlyPrices> MonthlyPrices { get; set; }
    }
}