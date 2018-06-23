using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using XML_WS_AgencyApp.Helpers;
using XML_WS_AgencyApp.Models;
using XML_WS_AgencyApp.Models_DTO;

namespace XML_WS_AgencyApp.Controllers
{
    public class AgentController : Controller
    {
        public static readonly string SOAP_URL = "http://localhost:9090/soap-api/agentEndpointService";

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

        // GET: MyBookingUnits
        public ActionResult MyBookingUnits()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                var repo = new BookingUnitRepository();
                long currentUserId = User.Identity.GetUserId<long>();
                var model = repo.GetMyBookingUnitsDisplayViewModel(currentUserId);
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

        // GET: LocalReservation
        public ActionResult LocalReservation(long bookingUnitId)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                BookingUnitsDisplayRepo budRepo = new BookingUnitsDisplayRepo();
                var model = new LocalReservationViewModel();
                model.BookingUnitId = bookingUnitId;
                model.BookingUnitName = budRepo.GetBookingUnitNameById(bookingUnitId);
                model.DateFrom = DateTime.Now;
                model.DateTo = DateTime.Now;
                return View(model);
            }
        }

        // GET: ClientMessaging
        public ActionResult ClientMessaging()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                MessagesDisplayRepo msgdRepo = new MessagesDisplayRepo();
                long currentUserId = User.Identity.GetUserId<long>();

                //mocking the initial messages
                msgdRepo.AddInitialMessages(currentUserId);

                var model = new ClientMessagingViewModel();
                model.ReceivedMessages = msgdRepo.GetMessages(currentUserId);
                return View(model);
            }
        }

        // GET: OpenMessage
        public ActionResult OpenMessage(long messageId)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                MessagesDisplayRepo msgdRepo = new MessagesDisplayRepo();
                long currentUserId = User.Identity.GetUserId<long>();
                OpenedMessageViewModel model = msgdRepo.GetOpennedMessage(messageId);
                return View(model);
            }
        }

        // GET: MyReservations
        public ActionResult MyReservations()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                ReservationsRepository resRepo = new ReservationsRepository();
                DisplayReservationsViewModel model = new DisplayReservationsViewModel();
                long currentUserId = User.Identity.GetUserId<long>();
                model.MyReservations = resRepo.GetReservations(currentUserId);
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
                    DTOHelper dtoHlp = new DTOHelper();
                    bool imgValidation = dtoHlp.ValidateImageTypes(anbuVM.Images);

                    if(imgValidation)
                    {
                        long curentUserId = User.Identity.GetUserId<long>();

                        //send a request on the main server and await for the response
                        MyRemoteServices.AgentEndpointPortClient aepc = new MyRemoteServices.AgentEndpointPortClient();
                        MyRemoteServices.addBookingUnitRequest abuRequest = dtoHlp.GetBookingUnitRequest(anbuVM, curentUserId);
                        MyRemoteServices.addBookingUnitResponse abuResponse = aepc.addBookingUnit(abuRequest);


                        if (abuResponse.responseWrapper.success)
                        {
                            //save localy
                            using (var ctx = new ApplicationDbContext())
                            {
                                //create a new booking unit with all the prerequisits
                                City myCity = ctx.Cities.FirstOrDefault(x => x.Id.ToString() == anbuVM.CityId);
                                ApplicationUser agentUsr = ctx.Users.FirstOrDefault(x => x.Id == curentUserId);
                                AccomodationType myAccType = ctx.AccomodationTypes.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationTypeId);
                                AccomodationCategory myAccCat = ctx.AccomodationCategories.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationCategoryId);

                                ICollection<BonusFeatures> myBonusFeatures = new List<BonusFeatures>();
                                foreach (var bfvm in anbuVM.BonusFeatures)
                                {
                                    if (bfvm.IsSelected)
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
                                    BonusFeatures = myBonusFeatures,
                                    MainServerId = (long?)abuResponse.responseWrapper.responseBody
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
                            TempData["error"] = abuResponse.responseWrapper.message;
                            return RedirectToAction("AddNewBookingUnit", "Agent");
                        }

                        TempData["success"] = "Successfully added a new booking unit";
                        return RedirectToAction("AgentPage", "Agent");
                    }
                    else
                    {
                        //image type exception
                        TempData["error"] = "Wrong image type, please try again";
                        return RedirectToAction("AddNewBookingUnit", "Agent");
                    }
                }
                else
                {
                    //invalid VM exception
                    TempData["error"] = "Some form atributes are incorrect";
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
                    DTOHelper dtoHlp = new DTOHelper();
                    MonthlyPrices_DTO mpDTO = dtoHlp.GetMonthlyPrices_DTO(mpVM);
                    var xmlHelper = new XMLHelper();
                    string xmlPayload = xmlHelper.SerializeToXml(mpDTO);

                    bool serverResp = true;

                    bool isUpdate = false;
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
                                double[] myMonths = new double[12];
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

                                isUpdate = true;
                            }

                            //save changes
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        //some error happened, retry
                        TempData["error"] = "Main server error";
                        return RedirectToAction("ManageMonhtlyPrices", "Agent", new { bookingUnitId = mpVM.BookingUnitId });
                    }

                    TempData["success"] = "Successfully ADDED a new monthly prices sheet for the year " + mpVM.Year;
                    if (isUpdate)
                        TempData["success"] = "Successfully UPDATED the monthly prices sheet for the year " + mpVM.Year;
                    return RedirectToAction("AgentPage", "Agent");
                }
                else
                {
                    //invalid VM exception
                    TempData["error"] = "Some form atributes are incorrect";
                    return RedirectToAction("ManageMonhtlyPrices", "Agent", new { bookingUnitId = mpVM.BookingUnitId});
                }
            }
        }

        public async Task<ActionResult> AddLocalReservation(LocalReservationViewModel lrVM)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                if (ModelState.IsValid)
                {
                    int totalDays = (lrVM.DateTo - lrVM.DateFrom).Days;
                    bool isCorrectDate = true;
                    var now = DateTime.Now;
                    var checkDate = new DateTime(now.Year, now.Month, now.Day);
                    if (checkDate > lrVM.DateFrom)
                        isCorrectDate = false;

                    if(totalDays <= 30 && isCorrectDate)
                    {
                        //send a request on the main server and await for the response
                        DTOHelper dtoHlp = new DTOHelper();
                        Reservation_DTO resDTO = dtoHlp.GetReservation_DTO(lrVM);
                        var xmlHelper = new XMLHelper();
                        string xmlPayload = xmlHelper.SerializeToXml(resDTO);

                        bool serverResp = true;

                        if (serverResp)
                        {
                            //save localy
                            using (var ctx = new ApplicationDbContext())
                            {
                                BookingUnit myUnit = ctx.BookingUnits.FirstOrDefault(x => x.Id == lrVM.BookingUnitId);
                                TotalPriceCalculator totPrCalc = new TotalPriceCalculator();
                                double totalPrice = totPrCalc.CalculateTotalPrice(lrVM.BookingUnitId, lrVM.DateFrom, lrVM.DateTo);

                                Reservation localReservation = new Reservation
                                {
                                    ReservationStatus = ReservationStatus.WAITING,
                                    SubjectName = lrVM.ReserveeFirstName,
                                    SubjectSurname = lrVM.ReserveeLastName,
                                    From = lrVM.DateFrom,
                                    To = lrVM.DateTo,
                                    BookingUnit = myUnit,
                                    TotalPrice = totalPrice
                                };

                                //add the new item
                                ctx.Reservations.Add(localReservation);

                                //save changes
                                ctx.SaveChanges();
                            }
                        }
                        else
                        {
                            //some error happened, retry
                            TempData["error"] = "Main server error";
                            return RedirectToAction("LocalReservation", "Agent", new { bookingUnitId = lrVM.BookingUnitId });
                        }

                        TempData["success"] = "Successfully added a local reservation of the unit: " + lrVM.BookingUnitName;
                        return RedirectToAction("AgentPage", "Agent");
                    }
                    else
                    {
                        //max days limit or begin date exception
                        TempData["error"] = "The begin date must be today's date or greater. The maximum number of days for one reservation is 30";
                        return RedirectToAction("LocalReservation", "Agent", new { bookingUnitId = lrVM.BookingUnitId });
                    }
                }
                else
                {
                    //invalid VM exception
                    TempData["error"] = "Some form atributes are incorrect";
                    return RedirectToAction("LocalReservation", "Agent", new { bookingUnitId = lrVM.BookingUnitId });
                }
            }
        }

        public async Task<ActionResult> SendMessageResponse(OpenedMessageViewModel omVM)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                if(ModelState.IsValid)
                {
                    //send a request on the main server and await for the response
                    long curentUserId = User.Identity.GetUserId<long>();
                    DTOHelper dtoHlp = new DTOHelper();
                    Message_DTO msgDTO = dtoHlp.GetMessage_DTO(omVM, curentUserId);
                    var xmlHelper = new XMLHelper();
                    string xmlPayload = xmlHelper.SerializeToXml(msgDTO);

                    bool serverResp = true;

                    if (serverResp)
                    {
                        //save localy
                        using (var ctx = new ApplicationDbContext())
                        {
                            ResponseMessage respMsg = new ResponseMessage
                            {
                                Content = omVM.ResponseContent                                
                            };
                            ctx.ResponseMessages.Add(respMsg);

                            Message msg = ctx.Messages.FirstOrDefault(x => x.Id == omVM.Id);
                            msg.HasResponse = true;
                            msg.ResponseMessage = respMsg;

                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        //some error happened, retry
                        TempData["error"] = "Main server error";
                        return RedirectToAction("OpenMessage", "Agent", new { messageId = omVM.Id });
                    }

                    TempData["success"] = "Successfully sent the response message to: " + omVM.SenderUserName;
                    return RedirectToAction("OpenMessage", "Agent", new { messageId = omVM.Id });
                }
                else
                {
                    //invalid VM exception
                    TempData["error"] = "Some form atributes are incorrect";
                    return RedirectToAction("OpenMessage", "Agent", new { messageId = omVM.Id });
                }
            }
        }

        public async Task<ActionResult> ConfirmReservation(long reservationID)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                if (ModelState.IsValid)
                {
                    //send a request on the main server and await for the response
                    DTOHelper dtoHlp = new DTOHelper();
                    ReservationStatus_DTO rsDTO = dtoHlp.GetReservationStatus_DTO(reservationID, ReservationStatus.CONFIRMED);
                    var xmlHelper = new XMLHelper();
                    string xmlPayload = xmlHelper.SerializeToXml(rsDTO);

                    bool serverResp = true;

                    if (serverResp)
                    {
                        //save localy
                        using (var ctx = new ApplicationDbContext())
                        {
                            Reservation res = ctx.Reservations.FirstOrDefault(x => x.Id == reservationID);
                            res.ReservationStatus = ReservationStatus.CONFIRMED;
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        //some error happened, retry
                        TempData["error"] = "Main server error";
                        return RedirectToAction("MyReservations", "Agent");
                    }

                    TempData["success"] = "Successfully confirmed the reservation";
                    return RedirectToAction("MyReservations", "Agent");
                }
                else
                {
                    //invalid VM exception
                    TempData["error"] = "Some form atributes are incorrect";
                    return RedirectToAction("MyReservations", "Agent");
                }
            }
        }

        public async Task<ActionResult> CancelReservation(long reservationID)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                if (ModelState.IsValid)
                {
                    //send a request on the main server and await for the response
                    DTOHelper dtoHlp = new DTOHelper();
                    ReservationStatus_DTO rsDTO = dtoHlp.GetReservationStatus_DTO(reservationID, ReservationStatus.CANCELED);
                    var xmlHelper = new XMLHelper();
                    string xmlPayload = xmlHelper.SerializeToXml(rsDTO);

                    bool serverResp = true;

                    if (serverResp)
                    {
                        //save localy
                        using (var ctx = new ApplicationDbContext())
                        {
                            Reservation res = ctx.Reservations.FirstOrDefault(x => x.Id == reservationID);
                            res.ReservationStatus = ReservationStatus.CANCELED;
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        //some error happened, retry
                        TempData["error"] = "Main server error";
                        return RedirectToAction("MyReservations", "Agent");
                    }

                    TempData["success"] = "Successfully cancelled the reservation";
                    return RedirectToAction("MyReservations", "Agent");
                }
                else
                {
                    //invalid VM exception
                    TempData["error"] = "Some form atributes are incorrect";
                    return RedirectToAction("MyReservations", "Agent");
                }
            }
        }
    }
}