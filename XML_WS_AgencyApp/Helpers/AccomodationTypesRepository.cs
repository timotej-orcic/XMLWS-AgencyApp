﻿using System.Collections.Generic;
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
                List<SelectListItem> accTypes = ctx.AccomodationTypes.AsNoTracking()
                     .OrderBy(x => x.Name)
                         .Select(x =>
                         new SelectListItem
                         {
                             Value = x.Id.ToString(),
                             Text = x.Name
                         }).ToList();
                var accTypesTip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select accomodation type ---"
                };
                accTypes.Insert(0, accTypesTip);
                return new SelectList(accTypes, "Value", "Text");
            }
        }
    }
}