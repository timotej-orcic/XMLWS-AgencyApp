using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using XML_WS_AgencyApp.Helpers;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Controllers
{
    public class AgentController : Controller
    {
        // GET: AgentPage
        public ActionResult AgentPage()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
                return View();
        }

        // GET: AddNewBookingUnit
        public ActionResult AddNewBookingUnit()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                var repo = new BookingUnitRepository();
                var model = repo.CreateBookingUnitViewModel();
                return View(model);
            }                
        }

        [HttpGet]
        public ActionResult GetCitiesByCountryId(string countryId)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                if(!string.IsNullOrWhiteSpace(countryId))
                {
                    var repo = new CitiesRepository();
                    IEnumerable<SelectListItem> cities = repo.GetCities(countryId);
                    return Json(cities, JsonRequestBehavior.AllowGet);
                }
                else
                    return null;
            }
        }

        public async Task<ActionResult> AddBookingUnit(AddNewBookingUnitViewModel anbuVM)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                return null;
            }
        }
    }
}