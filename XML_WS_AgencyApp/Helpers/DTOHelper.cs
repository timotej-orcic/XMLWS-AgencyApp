using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using XML_WS_AgencyApp.Models;
using XML_WS_AgencyApp.Models_DTO;

namespace XML_WS_AgencyApp.Helpers
{
    public class DTOHelper
    {
        public bool ValidateImageTypes(HttpPostedFileBase[] images)
        {
            bool retVal = true;

            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            foreach(var img in images)
            {
                if (!validImageTypes.Contains(img.ContentType))
                {
                    retVal = false;
                    break;
                }
            }

            return retVal;
        }

        /*public MyRemoteServices.addBookingUnitRequest GetBookingUnitRequest(AddNewBookingUnitViewModel anbuVM, long curentUserId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long cityId = (long)ctx.Cities.FirstOrDefault(x => x.Id.ToString() == anbuVM.CityId).MainServerId;
                long accCatId = (long)ctx.AccomodationCategories.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationCategoryId).MainServerId;
                long accTypeId = (long)ctx.AccomodationTypes.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationTypeId).MainServerId;
                long agentId = (long)ctx.Users.FirstOrDefault(x => x.Id == curentUserId).MainServerId;

                int bfCnt = anbuVM.BonusFeatures.Count;
                long?[] bonusFeaturesIds = new long?[bfCnt];
                for(int i = 0; i < bfCnt; i++)
                {
                    var bonusFeature = anbuVM.BonusFeatures[i];
                    if (bonusFeature.IsSelected)
                    {
                        long currentId = bonusFeature.Id;
                        long bfId = (long)ctx.BonusFeatures.FirstOrDefault(x => x.Id == currentId).MainServerId;
                        bonusFeaturesIds[i] = bfId;
                    }
                }

                int imgCnt = anbuVM.Images.Length;
                for(int i = 0; i < imgCnt; i++)
                {
                    var file = anbuVM.Images[i];
                    string imgName = file.FileName;
                    byte[] thePictureAsBytes = new byte[file.ContentLength];
                    using (BinaryReader reader = new BinaryReader(file.InputStream))
                    {
                        thePictureAsBytes = reader.ReadBytes(file.ContentLength);
                    }
                    string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                    base64ImagesList[i] = new MyRemoteServices.bookingUnitDTOEntry
                    {
                        key = imgName,
                        value = thePictureDataAsString
                    };
                }


                MyRemoteServices.addBookingUnitRequest retObj = new MyRemoteServices.addBookingUnitRequest
                {
                    
                };

                return retObj;
            }
        }*/

        public MonthlyPrices_DTO GetMonthlyPrices_DTO(MonthlyPricesViewModel mpVM)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long bookingUnitId = (long)ctx.BookingUnits.FirstOrDefault(x => x.Id == mpVM.BookingUnitId).MainServerId;

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


                MonthlyPrices_DTO retObj = new MonthlyPrices_DTO
                {
                    BookingUnitMainServerId = bookingUnitId,
                    Year = mpVM.Year,
                    MonthlyPrices = myMonths
                };

                return retObj;
            }
        }

        public Reservation_DTO GetReservation_DTO(LocalReservationViewModel lrVM)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long bookingUnitId = (long)ctx.BookingUnits.FirstOrDefault(x => x.Id == lrVM.BookingUnitId).MainServerId;
                string dateFrom = string.Format("{0}/{1}/{2}", lrVM.DateFrom.Year, lrVM.DateFrom.Month, lrVM.DateFrom.Day);
                string dateTo = string.Format("{0}/{1}/{2}", lrVM.DateTo.Year, lrVM.DateTo.Month, lrVM.DateTo.Day);

                Reservation_DTO retObj = new Reservation_DTO
                {
                    BookingUnitMainServerId = bookingUnitId,
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    ReserveeFirstName = lrVM.ReserveeFirstName,
                    ReserveeLastName = lrVM.ReserveeLastName
                };

                return retObj;
            }
        }

        public Message_DTO GetMessage_DTO(OpenedMessageViewModel omVM, long curentUserId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long agentId = (long)ctx.Users.FirstOrDefault(x => x.Id == curentUserId).MainServerId;
                long userReceiverId = (long)ctx.RegisteredUsersInfo.FirstOrDefault(x => x.Id == omVM.SenderId).MainServerId;

                Message_DTO retObj = new Message_DTO
                {
                    Content = omVM.Content,
                    SenderAgentMainServerId = agentId,
                    ReceiverUserMainServerId = userReceiverId
                };

                return retObj;
            }
        }

        public ReservationStatus_DTO GetReservationStatus_DTO(long reservationId, ReservationStatus rStatus)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long serverId = (long)ctx.Reservations.FirstOrDefault(x => x.Id == reservationId).MainServerId;

                ReservationStatus_DTO retObj = new ReservationStatus_DTO
                {
                    ReservationMainServerId = serverId,
                    ReservationStatus = rStatus
                };

                return retObj;
            }
        }
    }
}