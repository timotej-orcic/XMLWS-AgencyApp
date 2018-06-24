using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class SynchronizationHelper
    {
        public static string DoSynchronization(MyRemoteServices.SinchronizationObject syncObj)
        {
            string syncLog = "***************BEGIN LOG***************" + Environment.NewLine;

            MyRemoteServices.BonusFeature[] incomingBonusFeatures = syncObj.bonusFeaturesList;
            MyRemoteServices.AccomodationCategory[] incomingAccCats = syncObj.accCatsList;
            MyRemoteServices.AccomodationType[] incomingAccTypes = syncObj.accTypesList;
            MyRemoteServices.Country[] incomingCountries = syncObj.countriesList;
            MyRemoteServices.City[] incomingCities = syncObj.citiesList;
            MyRemoteServices.RegUsrMessage[] incomingMessages = syncObj.regUserMessagesList;
            MyRemoteServices.MainServerReservation[] incomingReservations = syncObj.reservationsList;
            using (var ctx = new ApplicationDbContext())
            {
                if(incomingBonusFeatures != null)
                {
                    foreach (var bf in incomingBonusFeatures)
                    {
                        bool actionHappened = false;
                        var localBfData = ctx.BonusFeatures.FirstOrDefault(x => x.MainServerId == bf.mainServerId);
                        if (localBfData == null)
                        {
                            BonusFeatures localBf = new BonusFeatures
                            {
                                MainServerId = bf.mainServerId,
                                Name = bf.name
                            };
                            ctx.BonusFeatures.Add(localBf);
                            actionHappened = true;
                            syncLog += string.Format("Added a new BonusFeature: {0}" + Environment.NewLine, localBf.Name);
                        }
                        else
                        {
                            if (bf.name != localBfData.Name)
                            {
                                syncLog += string.Format("Edited the {0} BonusFeature's name property from {1} to {2}" + Environment.NewLine, localBfData.Name, localBfData.Name, bf.name);
                                localBfData.Name = bf.name;
                                actionHappened = true;
                            }
                        }

                        if (actionHappened)
                            syncLog += TrySave(ctx);
                    }

                    syncLog += "=============================================================" + Environment.NewLine;
                }

                if(incomingAccCats != null)
                {
                    foreach (var accCat in incomingAccCats)
                    {
                        bool actionHappened = false;
                        var localAccCatData = ctx.AccomodationCategories.FirstOrDefault(x => x.MainServerId == accCat.mainServerId);
                        if (localAccCatData == null)
                        {
                            AccomodationCategory localAccCat = new AccomodationCategory
                            {
                                MainServerId = accCat.mainServerId,
                                Name = accCat.name
                            };
                            ctx.AccomodationCategories.Add(localAccCat);
                            actionHappened = true;
                            syncLog += string.Format("Added a new AccomodationCategory: {0}" + Environment.NewLine, localAccCat.Name);
                        }
                        else
                        {
                            if (localAccCatData.Name != accCat.name)
                            {
                                syncLog += string.Format("Edited the {0} AccomodationCategory's name property from {1} to {2}" + Environment.NewLine, localAccCatData.Name, localAccCatData.Name, accCat.name);
                                localAccCatData.Name = accCat.name;
                                actionHappened = true;
                            }
                        }

                        if (actionHappened)
                            syncLog += TrySave(ctx);
                    }

                    syncLog += "=============================================================" + Environment.NewLine;
                }

                if(incomingAccTypes != null)
                {
                    foreach (var accType in incomingAccTypes)
                    {
                        bool actionHappened = false;
                        var localAccTypeData = ctx.AccomodationTypes.FirstOrDefault(x => x.MainServerId == accType.mainServerId);
                        if (localAccTypeData == null)
                        {
                            AccomodationType localAccType = new AccomodationType
                            {
                                MainServerId = accType.mainServerId,
                                Name = accType.name
                            };
                            ctx.AccomodationTypes.Add(localAccType);
                            actionHappened = true;
                            syncLog += string.Format("Added a new AccomodationType: {0}" + Environment.NewLine, localAccType.Name);
                        }
                        else
                        {
                            if (localAccTypeData.Name != accType.name)
                            {
                                syncLog += string.Format("Edited the {0} AccomodationType's name property from {1} to {2}" + Environment.NewLine, localAccTypeData.Name, localAccTypeData.Name, accType.name);
                                localAccTypeData.Name = accType.name;
                                actionHappened = true;
                            }
                        }

                        if (actionHappened)
                            syncLog += TrySave(ctx);
                    }

                    syncLog += "=============================================================" + Environment.NewLine;
                }

                if(incomingCountries != null)
                {
                    foreach (var cntry in incomingCountries)
                    {
                        bool actionHappened = false;
                        var localCntryData = ctx.Countries.FirstOrDefault(x => x.MainServerId == cntry.mainServerId);
                        if (localCntryData == null)
                        {
                            Country localCntry = new Country
                            {
                                MainServerId = cntry.mainServerId,
                                Name = cntry.name
                            };
                            ctx.Countries.Add(localCntry);
                            actionHappened = true;
                            syncLog += string.Format("Added a new Country: {0}" + Environment.NewLine, localCntry.Name);
                        }
                        else
                        {
                            if (localCntryData.Name != cntry.name)
                            {
                                syncLog += string.Format("Edited the {0} Country's name property from {1} to {2}" + Environment.NewLine, localCntryData.Name, localCntryData.Name, cntry.name);
                                localCntryData.Name = cntry.name;
                                actionHappened = true;
                            }
                        }

                        if (actionHappened)
                            syncLog += TrySave(ctx);
                    }

                    syncLog += "=============================================================" + Environment.NewLine;
                }

                if(incomingCities != null)
                {
                    foreach (var city in incomingCities)
                    {
                        bool actionHappened = false;
                        var localCityData = ctx.Cities.FirstOrDefault(x => x.MainServerId == city.mainServerId);
                        if (localCityData == null)
                        {
                            var cntry = ctx.Countries.FirstOrDefault(x => x.MainServerId == city.country.mainServerId);
                            City localCity = new City
                            {
                                MainServerId = city.mainServerId,
                                Name = city.name,
                                PostalCode = city.postalCode,
                                Country = cntry
                            };
                            ctx.Cities.Add(localCity);
                            actionHappened = true;
                            syncLog += string.Format("Added a new City: {0}" + Environment.NewLine, localCity.Name);
                        }
                        else
                        {
                            if (localCityData.Name != city.name)
                            {
                                syncLog += string.Format("Edited the {0} City's name property from {1} to {2}" + Environment.NewLine, localCityData.Name, localCityData.Name, city.name);
                                localCityData.Name = city.name;
                                actionHappened = true;
                            }
                            if (localCityData.PostalCode != city.postalCode)
                            {
                                syncLog += string.Format("Edited the {0} City's postalCode property from {1} to {2}" + Environment.NewLine, localCityData.Name, localCityData.PostalCode, city.postalCode);
                                localCityData.PostalCode = city.postalCode;
                                actionHappened = true;
                            }
                            if (localCityData.Country.MainServerId != city.country.mainServerId)
                            {
                                var newCountry = ctx.Countries.FirstOrDefault(x => x.MainServerId == city.country.mainServerId);
                                if (newCountry == null)
                                    syncLog += string.Format("Error while trying to edit the {0} City's country property: No Country with such id is found." + Environment.NewLine, localCityData.Name);
                                else
                                {
                                    syncLog += string.Format("Edited the {0} City's country property from {1} to {2}" + Environment.NewLine, localCityData.Name, localCityData.Country.Name, city.country.name);
                                    localCityData.Country = newCountry;
                                    actionHappened = true;
                                }
                            }
                        }

                        if (actionHappened)
                            syncLog += TrySave(ctx);
                    }

                    syncLog += "=============================================================" + Environment.NewLine;
                }

                if(incomingMessages != null)
                {
                    foreach (var msg in incomingMessages)
                    {
                        bool actionHappened = false;
                        var localMsgData = ctx.Messages.FirstOrDefault(x => x.MainServerId == msg.mainServerId);
                        if (localMsgData == null)
                        {
                            var localRegUserInfo = ctx.RegisteredUsersInfo.FirstOrDefault(x => x.MainServerId == msg.regUsrInfo.mainServerId);
                            if (localRegUserInfo == null)
                            {
                                localRegUserInfo = new RegisteredUserInfo
                                {
                                    MainServerId = msg.regUsrInfo.mainServerId,
                                    UserName = msg.regUsrInfo.userName
                                };
                                ctx.RegisteredUsersInfo.Add(localRegUserInfo);
                                actionHappened = true;
                                syncLog += string.Format("Added a new RegisteredUserInfo: {0}" + Environment.NewLine, localRegUserInfo.UserName);
                            }

                            var localAgent = ctx.Users.FirstOrDefault(x => x.MainServerId == msg.agentUserMainServerId);
                            if (localAgent == null)
                                syncLog += string.Format("Error while trying to add a new message: agent user not found." + Environment.NewLine);
                            else
                            {
                                Message localMsg = new Message
                                {
                                    MainServerId = msg.mainServerId,
                                    AgentUserId = localAgent.Id,
                                    Content = msg.content,
                                    RegisteredUserInfo = localRegUserInfo,
                                    IsRead = false,
                                    HasResponse = false
                                };
                                ctx.Messages.Add(localMsg);
                                actionHappened = true;
                                syncLog += string.Format("Added a new Message" + Environment.NewLine);
                            }
                        }

                        if (actionHappened)
                            syncLog += TrySave(ctx);
                    }

                    syncLog += "=============================================================" + Environment.NewLine;
                }

                if(incomingReservations != null)
                {
                    foreach (var res in incomingReservations)
                    {
                        bool actionHappened = false;
                        var localReservationData = ctx.Reservations.FirstOrDefault(x => x.MainServerId == res.mainServerId);
                        if (localReservationData == null)
                        {
                            var bookingUnitLocal = ctx.BookingUnits.FirstOrDefault(x => x.MainServerId == res.bookingUnitMainServerId);
                            if (bookingUnitLocal == null)
                                syncLog += string.Format("Error while trying to add a new reservation: bookingUnit not found." + Environment.NewLine);
                            else
                            {
                                Reservation localReservation = new Reservation
                                {
                                    MainServerId = res.mainServerId,
                                    From = res.from,
                                    To = res.to,
                                    ReservationStatus = (ReservationStatus)res.reservationStatus,
                                    SubjectName = res.subjectName,
                                    SubjectSurname = res.subjectSurname,
                                    TotalPrice = res.totalPrice,
                                    BookingUnit = bookingUnitLocal
                                };
                                ctx.Reservations.Add(localReservation);
                                actionHappened = true;
                                syncLog += string.Format("Added a new Reservation of the booking unit: {0}" + Environment.NewLine, bookingUnitLocal.Name);
                            }
                        }
                        else
                        {
                            if (localReservationData.ReservationStatus != (ReservationStatus)res.reservationStatus)
                            {
                                var bUnit = ctx.BookingUnits.FirstOrDefault(x => x.MainServerId == res.bookingUnitMainServerId);
                                syncLog += string.Format("Edited the reservation status from {0} to {1} for the Booking unit: {2}" + Environment.NewLine, localReservationData.ReservationStatus.ToString(), res.reservationStatus.ToString(), bUnit.Name);
                                localReservationData.ReservationStatus = (ReservationStatus)res.reservationStatus;
                                actionHappened = true;
                            }
                        }

                        if (actionHappened)
                            syncLog += TrySave(ctx);
                    }

                    syncLog += "***************END LOG***************";
                }
            }

            return syncLog;
        }

        public static string TrySave(ApplicationDbContext ctx)
        {
            string retMsg = "";
            bool hasError = false;

            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                retMsg += string.Format("The above action failed with details:" + Environment.NewLine);
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        retMsg += string.Format("{0}:{1}" + Environment.NewLine, validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                    }
                }

                hasError = true;
            }

            if (!hasError)
                retMsg += "Success!" + Environment.NewLine;

            return retMsg;
        }
    }
}