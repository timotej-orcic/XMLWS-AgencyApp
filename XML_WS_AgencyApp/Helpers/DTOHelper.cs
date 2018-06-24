using System;
using System.IO;
using System.Linq;
using System.Web;
using XML_WS_AgencyApp.Models;

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

        public MyRemoteServices.addBookingUnitRequest GetBookingUnitRequest(AddNewBookingUnitViewModel anbuVM, long curentUserId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long cityId = (long)ctx.Cities.FirstOrDefault(x => x.Id.ToString() == anbuVM.CityId).MainServerId;
                long accCatId = (long)ctx.AccomodationCategories.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationCategoryId).MainServerId;
                long accTypeId = (long)ctx.AccomodationTypes.FirstOrDefault(x => x.Id.ToString() == anbuVM.AccomodationTypeId).MainServerId;
                long agentId = (long)ctx.Users.FirstOrDefault(x => x.Id == curentUserId).MainServerId;

                long[] bonusFeaturesIds;
                if (anbuVM.BonusFeatures != null)
                {
                    int bfCnt = anbuVM.BonusFeatures.Count;
                    bonusFeaturesIds = new long[bfCnt];
                    for (int i = 0; i < bfCnt; i++)
                    {
                        var bonusFeature = anbuVM.BonusFeatures[i];
                        if (bonusFeature.IsSelected)
                        {
                            long currentId = bonusFeature.Id;
                            long bfId = (long)ctx.BonusFeatures.FirstOrDefault(x => x.Id == currentId).MainServerId;
                            bonusFeaturesIds[i] = bfId;
                        }
                    }
                }
                else
                    bonusFeaturesIds = null;

                int imgCnt = anbuVM.Images.Length;
                MyRemoteServices.hMapStringStringElement[] base64ImagesList = new MyRemoteServices.hMapStringStringElement[imgCnt];
                for (int i = 0; i < imgCnt; i++)
                {
                    var file = anbuVM.Images[i];
                    string imgName = file.FileName;
                    byte[] thePictureAsBytes = new byte[file.ContentLength];
                    using (BinaryReader reader = new BinaryReader(file.InputStream))
                    {
                        thePictureAsBytes = reader.ReadBytes(file.ContentLength);
                    }
                    string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                    base64ImagesList[i] = new MyRemoteServices.hMapStringStringElement
                    {
                        key = imgName,
                        value = thePictureDataAsString
                    };
                }

                MyRemoteServices.BookingUnit unit = new MyRemoteServices.BookingUnit
                {
                    cityMainServerId = cityId,
                    accCategoryMainServerId = accCatId,
                    accTypeMainServerId = accTypeId,
                    address = anbuVM.Address,
                    agentMainServerId = agentId,
                    bonusFeaturesMainServerIds = bonusFeaturesIds,
                    description = anbuVM.Description,
                    name = anbuVM.Name,
                    peopleNo = anbuVM.PeopleNo,
                    base64ImagesList = base64ImagesList
                };

                MyRemoteServices.addBookingUnitRequest retObj = new MyRemoteServices.addBookingUnitRequest
                {
                    bookingUnit = unit
                };

                return retObj;
            }
        }

        public MyRemoteServices.manageMonthlyPricesRequest GetMonthlyPricesRequest(MonthlyPricesViewModel mpVM)
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

                MyRemoteServices.MonthlyPrices prices = new MyRemoteServices.MonthlyPrices
                {
                    mainServerId = bookingUnitId,
                    monthlyPrices = myMonths,
                    year = mpVM.Year
                };

                MyRemoteServices.manageMonthlyPricesRequest retObj = new MyRemoteServices.manageMonthlyPricesRequest
                {
                    monthlyPrice = prices
                };

                return retObj;
            }
        }

        public MyRemoteServices.addLocalReservationRequest GetLocalReservationRequest(LocalReservationViewModel lrVM)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long bookingUnitId = (long)ctx.BookingUnits.FirstOrDefault(x => x.Id == lrVM.BookingUnitId).MainServerId;
                string dateFrom = string.Format("{0}/{1}/{2}", lrVM.DateFrom.Year, lrVM.DateFrom.Month, lrVM.DateFrom.Day);
                string dateTo = string.Format("{0}/{1}/{2}", lrVM.DateTo.Year, lrVM.DateTo.Month, lrVM.DateTo.Day);

                MyRemoteServices.Reservation resData = new MyRemoteServices.Reservation
                {
                    bookingUnitMainServerId = bookingUnitId,
                    dateFrom = dateFrom,
                    dateTo = dateTo,
                    reserveeFirstName = lrVM.ReserveeFirstName,
                    reserveeLastName = lrVM.ReserveeLastName
                };

                MyRemoteServices.addLocalReservationRequest retObj = new MyRemoteServices.addLocalReservationRequest
                {
                    localReservation = resData
                };

                return retObj;
            }
        }

        public MyRemoteServices.sendMessageRequest GetMessageRequest(OpenedMessageViewModel omVM, long curentUserId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long agentId = (long)ctx.Users.FirstOrDefault(x => x.Id == curentUserId).MainServerId;
                long userReceiverId = (long)ctx.RegisteredUsersInfo.FirstOrDefault(x => x.Id == omVM.SenderId).MainServerId;

                MyRemoteServices.Message msgData = new MyRemoteServices.Message
                {
                    content = omVM.Content,
                    senderAgentMainServerId = agentId,
                    receiverUserMainServerId = userReceiverId
                };

                MyRemoteServices.sendMessageRequest retObj = new MyRemoteServices.sendMessageRequest
                {
                    message = msgData
                };

                return retObj;
            }
        }

        public MyRemoteServices.confirmReservationRequest GetConfirmReservationRequest(long reservationId, ReservationStatus rStatus)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long serverId = (long)ctx.Reservations.FirstOrDefault(x => x.Id == reservationId).MainServerId;

                MyRemoteServices.ReservationLite resData = new MyRemoteServices.ReservationLite
                {
                    reservationMainServerId = serverId,
                    reservationStatus = (MyRemoteServices.ReservationStatus)rStatus                    
                };

                MyRemoteServices.confirmReservationRequest retObj = new MyRemoteServices.confirmReservationRequest
                {
                    reservationLite = resData
                };

                return retObj;
            }
        }

        public MyRemoteServices.cancelReservationRequest GetCancelReservationRequest(long reservationId, ReservationStatus rStatus)
        {
            using (var ctx = new ApplicationDbContext())
            {
                long serverId = (long)ctx.Reservations.FirstOrDefault(x => x.Id == reservationId).MainServerId;

                MyRemoteServices.ReservationLite resData = new MyRemoteServices.ReservationLite
                {
                    reservationMainServerId = serverId,
                    reservationStatus = (MyRemoteServices.ReservationStatus)rStatus
                };

                MyRemoteServices.cancelReservationRequest retObj = new MyRemoteServices.cancelReservationRequest
                {
                    reservationLite = resData
                };

                return retObj;
            }
        }
    }
}