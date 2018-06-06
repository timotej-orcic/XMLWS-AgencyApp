﻿using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        public Initializer()
        {
        }

        protected override void Seed(ApplicationDbContext ctx)
        {
            Country srb = new Country
            {
                Name = "Srbija"
            };

            ctx.Countries.Add(srb);

            Country hr = new Country
            {
                Name = "Hrvatska"
            };

            ctx.Countries.Add(hr);

            City ns = new City
            {
                Name = "Novi Sad",
                PostalCode = "21000",
                Country = srb
            };

            ctx.Cities.Add(ns);

            City bg = new City
            {
                Name = "Beograd",
                PostalCode = "11000",
                Country = srb
            };

            ctx.Cities.Add(bg);

            City su = new City
            {
                Name = "Subotica",
                PostalCode = "45000",
                Country = srb
            };

            ctx.Cities.Add(su);

            City zg = new City
            {
                Name = "Zagreb",
                PostalCode = "10000",
                Country = hr
            };

            ctx.Cities.Add(zg);

            AccomodationType hotel = new AccomodationType
            {
                Name = "Hotel"
            };

            ctx.AccomodationTypes.Add(hotel);

            AccomodationType bnb = new AccomodationType
            {
                Name = "Bed & Breakfast"
            };

            ctx.AccomodationTypes.Add(bnb);

            AccomodationType apartment = new AccomodationType
            {
                Name = "Apartment"
            };

            ctx.AccomodationTypes.Add(apartment);

            ctx.SaveChanges();
        }
    }
}