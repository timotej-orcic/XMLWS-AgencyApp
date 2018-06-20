using System;
using System.Collections.Generic;
using System.Linq;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class ReservationsRepository
    {
        public List<ReservationViewModel> GetReservations(long currentUserId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Reservation> reservations = ctx.Reservations
                    .Include("BookingUnit.Agent")
                        .Include("BookingUnit.Pictures")
                            .Where(x => x.BookingUnit.Agent.Id == currentUserId)
                                .OrderBy(x => x.From)
                                    .ToList();

                List<ReservationViewModel> retList = new List<ReservationViewModel>();
                foreach (var r in reservations)
                {
                    bool canConfirm = false;
                    if (DateTime.Now >= r.From)
                        canConfirm = true;

                    retList.Add(
                        new ReservationViewModel
                        {
                            Id = r.Id,
                            BookingUnitName = r.BookingUnit.Name,
                            DateFrom = string.Format("{0}/{1}/{2}", r.From.Month, r.From.Day, r.From.Year),
                            DateTo = string.Format("{0}/{1}/{2}", r.To.Month, r.To.Day, r.To.Year),
                            ImgUrl = r.BookingUnit.Pictures.First().Value,
                            IsConfirmed = r.Confirmed,
                            ReserveeFullName = r.SubjectName + " " + r.SubjectSurname,
                            TotalPrice = r.TotalPrice,
                            CanBeConfirmed = canConfirm
                        }
                    );
                }

                return retList;
            }
        }
    }
}