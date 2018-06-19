using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
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

        //isto moze samo kroz glavni server, kako se dogovorimo
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

        // GET: MyBookingUnits
        public ActionResult MyBookingUnits()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                var repo = new BookingUnitRepository();
                var model = repo.GetMyBookingUnitsDisplayViewModel();
                return View(model);
            }
        }

        // GET: ManageMonthlyPrices
        public ActionResult ManageMonthlyPrices(long bookingUnitId)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                var model = new MonthlyPricesViewModel();
                model.BookingUnitId = bookingUnitId;
                return View(model);
            }
        }

        public async Task<ActionResult> AddBookingUnit(AddNewBookingUnitViewModel anbuVM)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                if(ModelState.IsValid)
                {
                    //send a request on the main server and await for the response
                    bool serverResp = true;

                    if(serverResp)
                    {
                        //save localy
                        using (var ctx = new ApplicationDbContext())
                        {
                            //create a new booking unit with all the prerequisits
                            City myCity = ctx.Cities.FirstOrDefault(x => x.Id.ToString() == anbuVM.CityId);
                            string curentUserId = User.Identity.GetUserId();
                            ApplicationUser agentUsr = ctx.Users.FirstOrDefault(x => x.Id.ToString() == curentUserId);
                            AccomodationType myAccType = ctx.AccomodationTypes.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationTypeId);
                            AccomodationCategory myAccCat = ctx.AccomodationCategories.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationCategoryId);

                            ICollection<BonusFeatures> myBonusFeatures = new List<BonusFeatures>();                       
                            foreach(var bfvm in anbuVM.BonusFeatures)
                            {
                                if(bfvm.IsSelected)
                                {
                                    var myBFeature = ctx.BonusFeatures.FirstOrDefault(x => x.Id == bfvm.Id);
                                    if (myBFeature != null)
                                        myBonusFeatures.Add(myBFeature);
                                }
                            }

                            BookingUnit newUnit = new BookingUnit
                            {
                                Name = anbuVM.Name,
                                Address = anbuVM.Address,
                                City = myCity,
                                Description = anbuVM.Description,
                                PeopleNo = anbuVM.PeopleNo,
                                Agent = agentUsr,
                                AccomodationType = myAccType,
                                AccomodationCategory = myAccCat,
                                BonusFeatures = myBonusFeatures
                            };

                            //add the new unit to the DBContext
                            ctx.BookingUnits.Add(newUnit);

                            //get uploaded images and save them on the server
                            var uploadDir = "~/uploadedImages";

                            foreach (var imgUpl in anbuVM.Images)
                            {
                                string newFileName = string.Concat(Path.GetFileNameWithoutExtension(imgUpl.FileName)
                                                            , DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss")
                                                                , Path.GetExtension(imgUpl.FileName)
                                                                    );
                                var imagePath = Path.Combine(Server.MapPath(uploadDir), newFileName);
                                imgUpl.SaveAs(imagePath);

                                var imageUrl = Path.Combine(uploadDir, newFileName);
                                BookingUnitPicture newPicture = new BookingUnitPicture
                                {
                                    BookingUnit = newUnit,
                                    Value = imageUrl
                                };
                                ctx.Pictures.Add(newPicture);
                            }

                            //save changes
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        //some error happened, retry

                    }

                    return RedirectToAction("AgentPage", "Agent");
                }
                else
                {
                    return RedirectToAction("AddNewBookingUnit", "Agent");
                }
            }
        }

        public async Task<ActionResult> AddMonthlyPrices(MonthlyPricesViewModel mpVM)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                if (ModelState.IsValid)
                {
                    //send a request on the main server and await for the response
                    bool serverResp = true;

                    if(serverResp)
                    {
                        //save localy
                        using (var ctx = new ApplicationDbContext())
                        {
                            //see if the monthly price sheet already exists
                            var queryData = ctx.MonthlyPrices
                                .Include("BookingUnit")
                                    .Where(x => x.BookingUnit.Id == mpVM.BookingUnitId && x.Year == mpVM.Year)
                                        .OrderBy(x => x.Month)
                                            .ToList();                                

                            if(queryData.Count == 0)
                            {
                                //if not, add a new one
                                BookingUnit myUnit = ctx.BookingUnits.FirstOrDefault(x => x.Id == mpVM.BookingUnitId);

                                //help structure for monthly prices
                                decimal[] myMonths = new decimal[12];
                                myMonths[0] = mpVM.JanuaryPrice;
                                myMonths[1] = mpVM.FebruaryPrice;
                                myMonths[2] = mpVM.MarchPrice;
                                myMonths[3] = mpVM.AprilPrice;
                                myMonths[4] = mpVM.MayPrice;
                                myMonths[5] = mpVM.JunePrice;
                                myMonths[6] = mpVM.JulyPrice;
                                myMonths[7] = mpVM.AugustPrice;
                                myMonths[8] = mpVM.SeptemberPrice;
                                myMonths[9] = mpVM.OctoberPrice;
                                myMonths[10] = mpVM.NovemberPrice;
                                myMonths[11] = mpVM.DecemberPrice;

                                for (int i = 0; i < 12; i++)
                                {
                                    MonthlyPrices newMp = new MonthlyPrices
                                    {
                                        Year = mpVM.Year,
                                        Month = i + 1,
                                        BookingUnit = myUnit,
                                        Amount = myMonths[i]
                                    };
                                    ctx.MonthlyPrices.Add(newMp);
                                }
                            }
                            else
                            {
                                //if yes, edit the existing one
                                foreach (var m in queryData)
                                {
                                    if (m.Month == 1)
                                        m.Amount = mpVM.JanuaryPrice;
                                    if (m.Month == 2)
                                        m.Amount = mpVM.FebruaryPrice;
                                    if (m.Month == 3)
                                        m.Amount = mpVM.MarchPrice;
                                    if (m.Month == 4)
                                        m.Amount = mpVM.AprilPrice;
                                    if (m.Month == 5)
                                        m.Amount = mpVM.MayPrice;
                                    if (m.Month == 6)
                                        m.Amount = mpVM.JunePrice;
                                    if (m.Month == 7)
                                        m.Amount = mpVM.JulyPrice;
                                    if (m.Month == 8)
                                        m.Amount = mpVM.AugustPrice;
                                    if (m.Month == 9)
                                        m.Amount = mpVM.SeptemberPrice;
                                    if (m.Month == 10)
                                        m.Amount = mpVM.OctoberPrice;
                                    if (m.Month == 11)
                                        m.Amount = mpVM.NovemberPrice;
                                    if (m.Month == 12)
                                        m.Amount = mpVM.DecemberPrice;
                                }
                            }

                            //save changes
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        //some error happened, retry

                    }

                    return RedirectToAction("AgentPage", "Agent");
                }
                else
                {
                    return RedirectToAction("ManageMonhtlyPrices", "Agent", new { bookingUnitId = mpVM.BookingUnitId});
                }
            }
        }
    }
}