using System.Collections.Generic;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        public Initializer()
        {
        }

        protected override void Seed(ApplicationDbContext context)
        {
            Country srbija = new Country
            {
                Name = "Srbija",
                MyCities = new List<City>()
            };

            City ns = new City
            {
                Name = "Novi Sad",
                PostalCode = "21000",
                MyBookingUnits = new List<BookingUnit>()
            };

            City bg = new City
            {
                Name = "Beograd",
                PostalCode = "11000",
                MyBookingUnits = new List<BookingUnit>()
            };

            BookingUnit duga = new BookingUnit
            {
                Address = "Fejes Klare 20",
                Description = "Odlican hostel",
                PeopleNo = 50,
                MyAccomodationCategories = new List<AccomodationCategory>(),
                MyAccomodationTypes = new List<AccomodationType>(),
                MyBonusFeatures = new List<BonusFeatures>(),
                MyMonthlyPrices = new List<MonthlyPrices>(),
                MyPictures = new List<BookingUnitPicture>(),
                MyReservations = new List<Reservation>()
            };

            context.BookingUnits.Add(duga);

            BookingUnit shereaton = new BookingUnit
            {
                Address = "Kralja Petra 22",
                Description = "Odlican hotel sa 5 zvezdica",
                PeopleNo = 360,
                MyAccomodationCategories = new List<AccomodationCategory>(),
                MyAccomodationTypes = new List<AccomodationType>(),
                MyBonusFeatures = new List<BonusFeatures>(),
                MyMonthlyPrices = new List<MonthlyPrices>(),
                MyPictures = new List<BookingUnitPicture>(),
                MyReservations = new List<Reservation>()
            };

            context.BookingUnits.Add(shereaton);

            ns.MyBookingUnits.Add(duga);
            srbija.MyCities.Add(ns);
            context.Cities.Add(ns);

            bg.MyBookingUnits.Add(shereaton);
            srbija.MyCities.Add(bg);
            context.Cities.Add(bg);

            context.Countries.Add(srbija);
        }
    }
}