using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class AccomodationCategoriesRepository
    {
        public IEnumerable<SelectListItem> GetAccomodationCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<SelectListItem> accCats = ctx.AccomodationCategories.AsNoTracking()
                     .OrderBy(x => x.Name)
                         .Select(x =>
                         new SelectListItem
                         {
                             Value = x.Id.ToString(),
                             Text = x.Name
                         }).ToList();
                var accCatTip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select accomodation category ---"
                };
                accCats.Insert(0, accCatTip);
                return new SelectList(accCats, "Value", "Text");
            }
        }
    }
}