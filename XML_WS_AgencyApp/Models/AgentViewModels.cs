using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace XML_WS_AgencyApp.Models
{
    public class AddNewBookingUnitViewModel
    {
        [Required(ErrorMessage = "The name field is required")]
        [MaxLength(60, ErrorMessage = "Max length is 60 characters")]
        public string Name { get; set; }

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

        [Required(ErrorMessage = "The accomodation category is required")]
        public string AccomodationCategoryId { get; set; }
        public IEnumerable<SelectListItem> AccomodationCategoriesList { get; set; }

        [Required(ErrorMessage = "At least one image is required")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase[] Images { get; set; }

        public List<BonusFeaturesViewModel> BonusFeatures { get; set; }
    }

    public class BonusFeaturesViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }

    public class DisplayBookingUnitsViewModel
    {
        public List<BookingUnitViewModel> MyBookingUnits { get; set; }
    }

    public class BookingUnitViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ImgUrl { get; set; }
    }

    public class MonthlyPricesViewModel
    {
        public long BookingUnitId { get; set; }

        [Required(ErrorMessage = "The year is required")]
        [Range(typeof(int), "2000", "2147483647", ErrorMessage = "The minimum year is 2000")]
        [RegularExpression(@"^([1-9]\d*|0)$", ErrorMessage = "Must be an integer")]
        public int Year { get; set; }

        [Required(ErrorMessage = "The January price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double JanuaryPrice { get; set; }

        [Required(ErrorMessage = "The February price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double FebruaryPrice { get; set; }

        [Required(ErrorMessage = "The March price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double MarchPrice { get; set; }

        [Required(ErrorMessage = "The April price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double AprilPrice { get; set; }

        [Required(ErrorMessage = "The May price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double MayPrice { get; set; }

        [Required(ErrorMessage = "The June price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double JunePrice { get; set; }

        [Required(ErrorMessage = "The July price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double JulyPrice { get; set; }

        [Required(ErrorMessage = "The August price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double AugustPrice { get; set; }

        [Required(ErrorMessage = "The September price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double SeptemberPrice { get; set; }

        [Required(ErrorMessage = "The October price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double OctoberPrice { get; set; }

        [Required(ErrorMessage = "The November price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double NovemberPrice { get; set; }

        [Required(ErrorMessage = "The December price is required")]
        [Range(typeof(double), "1", "1000000000", ErrorMessage = "The minimum unit price must be greater then 0")]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Must be a double number greater then 0")]
        public double DecemberPrice { get; set; }
    }

    public class LocalReservationViewModel
    {
        public long BookingUnitId { get; set; }

        public string BookingUnitName { get; set; }

        [MaxLength(60, ErrorMessage = "The maximum length is 60 characters")]
        public string ReserveeFirstName { get; set; }

        [MaxLength(60, ErrorMessage = "The maximum length is 60 characters")]
        public string ReserveeLastName { get; set; }

        [Required(ErrorMessage = "The start date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Must be a date-time input")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "The end date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Must be a date-time input")]
        [GreaterThan("DateFrom", ErrorMessage = "The end date must be greater then the start date")]
        public DateTime DateTo { get; set; }
    }

    public class ClientMessagingViewModel
    {
        public List<MessageViewModel> ReceivedMessages { get; set; }
    }

    public class MessageViewModel
    {
        public long Id { get; set; }

        public string SenderUserName { get; set; }

        public bool IsRead { get; set; }
    }

    public class OpenedMessageViewModel
    {
        public long Id { get; set; }

        public string SenderUserName { get; set; }

        public string Content { get; set; }

        [Required(ErrorMessage = "Response content is requred")]
        [MaxLength(1000, ErrorMessage = "Maximum length is 1000 characters")]
        public string ResponseContent { get; set; }

        public bool HasResponse { get; set; }
    }
}