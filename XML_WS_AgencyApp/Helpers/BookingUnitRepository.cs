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
            var bonusFeaturesRepo = new BonusFeaturesRepository();

            var bookingUnit = new AddNewBookingUnitViewModel()
            {
                CountriesList = countriesRepo.GetCountries(),
                CitiesList = citiesRepo.GetCities(),
                AccomodationTypesList = accTypesRepo.GetAccomodationTypes(),
                BonusFeatures = bonusFeaturesRepo.GetBonusFeatures()
            };
            return bookingUnit;
        }
    }
}