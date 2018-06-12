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
                                AccomodationCategory = null,
                                Pictures = new List<BookingUnitPicture>(),
                                BonusFeatures = myBonusFeatures
                            };

                            //check if the unit already exists localy
                            var localData = ctx.BookingUnits.FirstOrDefault(x => x.Name == anbuVM.Name);
                            if (localData == null)
                            {
                                //if no, add only
                                ctx.BookingUnits.Add(newUnit);
                            }
                            else
                            {
                                //if yes, delete the old one first and then add the new one
                                ctx.BookingUnits.Remove(localData);
                                ctx.BookingUnits.Add(newUnit);
                            }

                            //get uploaded images and save them on the server
                            var uploadDir = "~/uploadedImages";
                            List<BookingUnitPicture> myImages = new List<BookingUnitPicture>();

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
                                myImages.Add(newPicture);
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
    }
}