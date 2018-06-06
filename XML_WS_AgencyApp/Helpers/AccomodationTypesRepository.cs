using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class AccomodationTypesRepository
    {
        public IEnumerable<SelectListItem> GetAccomodationTypes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<SelectListItem> countries = ctx.AccomodationTypes.AsNoTracking()
                     .OrderBy(x => x.Name)
                         .Select(x =>
                         new SelectListItem
                         {
                             Value = x.Id.ToString(),
                             Text = x.Name
                         }).ToList();
                var countrytip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select accomodation type ---"
                };
                countries.Insert(0, countrytip);
                return new SelectList(countries, "Value", "Text");
            }
        }
    }
}