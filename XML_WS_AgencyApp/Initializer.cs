using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseAlways<ApplicationDbContext>
    {
        public Initializer()
        {
        }

        protected override void Seed(ApplicationDbContext ctx)
        {
            Country srb = new Country
            {
                Name = "Srbija",
                MainServerId = 0
            };

            ctx.Countries.Add(srb);

            Country hr = new Country
            {
                Name = "Hrvatska",
                MainServerId = 1
            };

            ctx.Countries.Add(hr);

            City ns = new City
            {
                Name = "Novi Sad",
                PostalCode = "21000",
                Country = srb,
                MainServerId = 0
            };

            ctx.Cities.Add(ns);

            City bg = new City
            {
                Name = "Beograd",
                PostalCode = "11000",
                Country = srb,
                MainServerId = 1
            };

            ctx.Cities.Add(bg);

            City su = new City
            {
                Name = "Subotica",
                PostalCode = "45000",
                Country = srb,
                MainServerId = 2
            };

            ctx.Cities.Add(su);

            City zg = new City
            {
                Name = "Zagreb",
                PostalCode = "10000",
                Country = hr,
                MainServerId = 3
            };

            ctx.Cities.Add(zg);

            AccomodationType hotel = new AccomodationType
            {
                Name = "Hotel",
                MainServerId = 0
            };

            ctx.AccomodationTypes.Add(hotel);

            AccomodationType bnb = new AccomodationType
            {
                Name = "Bed & Breakfast",
                MainServerId = 1
            };

            ctx.AccomodationTypes.Add(bnb);

            AccomodationType apartment = new AccomodationType
            {
                Name = "Apartment",
                MainServerId = 2
            };

            ctx.AccomodationTypes.Add(apartment);

            BonusFeatures wifi = new BonusFeatures
            {
                Name = "wi-fi",
                MainServerId = 0
            };

            ctx.BonusFeatures.Add(wifi);

            BonusFeatures tv = new BonusFeatures
            {
                Name = "TV",
                MainServerId = 1
            };

            ctx.BonusFeatures.Add(tv);

            BonusFeatures fridge = new BonusFeatures
            {
                Name = "Refrigirator",
                MainServerId = 2
            };

            ctx.BonusFeatures.Add(fridge);

            BonusFeatures ac = new BonusFeatures
            {
                Name = "Air conditioner",
                MainServerId = 3
            };

            ctx.BonusFeatures.Add(ac);

            AccomodationCategory noCat = new AccomodationCategory
            {
                Name = "uncategorized",
                MainServerId = 0
            };

            ctx.AccomodationCategories.Add(noCat);

            AccomodationCategory oneStar = new AccomodationCategory
            {
                Name = "1 star",
                MainServerId = 1
            };

            ctx.AccomodationCategories.Add(oneStar);

            AccomodationCategory twoStars = new AccomodationCategory
            {
                Name = "2 stars",
                MainServerId = 2
            };

            ctx.AccomodationCategories.Add(twoStars);

            AccomodationCategory threeStars = new AccomodationCategory
            {
                Name = "3 stars",
                MainServerId = 3
            };

            ctx.AccomodationCategories.Add(threeStars);

            AccomodationCategory fourStars = new AccomodationCategory
            {
                Name = "4 stars",
                MainServerId = 4
            };

            ctx.AccomodationCategories.Add(fourStars);

            AccomodationCategory fiveStars = new AccomodationCategory
            {
                Name = "5 stars",
                MainServerId = 5
            };

            ctx.AccomodationCategories.Add(fiveStars);

            RegisteredUserInfo regUsr = new RegisteredUserInfo
            {
                UserName = "Rikeeeh",
                MainServerId = null
            };

            ctx.RegisteredUsersInfo.Add(regUsr);

            ctx.SaveChanges();
        }
    }
}