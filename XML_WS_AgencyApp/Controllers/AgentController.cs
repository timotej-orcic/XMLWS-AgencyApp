using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XML_WS_AgencyApp.Controllers
{
    public class AgentController : Controller
    {
        // GET: AgentPage
        public ActionResult AgentPage()
        {
            return View();
        }

        // GET: AddNewBookingUnit
        public ActionResult AddNewBookingUnit()
        {
            return View();
        }
    }
}