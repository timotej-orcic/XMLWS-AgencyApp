using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class BookingUnitRepository
    {
        public AddNewBookingUnitViewModel CreateBookingUnitViewModel()
        {
            var countriesRepo = new CountriesRepository();
            var citiesRepo = new CitiesRepository();
            var accTypesRepo = new AccomodationTypesRepository();
            var accCategoriesRepo = new AccomodationCategoriesRepository();
            var bonusFeaturesRepo = new BonusFeaturesRepository();

            var bookingUnit = new AddNewBookingUnitViewModel()
            {
                CountriesList = countriesRepo.GetCountries(),
                CitiesList = citiesRepo.GetCities(),
                AccomodationTypesList = accTypesRepo.GetAccomodationTypes(),
                BonusFeatures = bonusFeaturesRepo.GetBonusFeatures(),
                AccomodationCategoriesList = accCategoriesRepo.GetAccomodationCategories()
            };
            return bookingUnit;
        }

        public DisplayBookingUnitsViewModel GetMyBookingUnitsDisplayViewModel(long curentUserId)
        {
            var bookingUnitsDisplayRepo = new BookingUnitsDisplayRepo();
            var bookingUnits = new DisplayBookingUnitsViewModel()
            {
                MyBookingUnits = bookingUnitsDisplayRepo.GetBookingUnits(curentUserId)
            };
            return bookingUnits;
        }
    }
}