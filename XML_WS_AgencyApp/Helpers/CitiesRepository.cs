using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class CitiesRepository
    {
        public IEnumerable<SelectListItem> GetCities()
        {
            List<SelectListItem> regions = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            return regions;
        }

        public IEnumerable<SelectListItem> GetCities(string countryId)
        {
            if (!string.IsNullOrWhiteSpace(countryId))
            {
                using (var ctx = new ApplicationDbContext())
                {
                    List<SelectListItem> cities = ctx.Cities.AsNoTracking()
                        .OrderBy(x => x.Name)
                        .Where(x => x.Country.Id.ToString() == countryId)
                        .Select(x =>
                           new SelectListItem
                           {
                               Value = x.Id.ToString(),
                               Text = x.Name
                           }).ToList();
                    return new SelectList(cities, "Value", "Text");
                }
            }
            return null;
        }
    }
}