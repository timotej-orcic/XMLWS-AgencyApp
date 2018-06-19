using System.Collections.Generic;
using System.Linq;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class BookingUnitsDisplayRepo
    {
        public List<BookingUnitViewModel> GetBookingUnits()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<BookingUnit> bookingUnits = ctx.BookingUnits
                    .Include("Pictures")
                        .Include("City.Country")
                            .AsNoTracking()
                                .OrderBy(x => x.Name)
                                    .ToList();

                List<BookingUnitViewModel> retList = new List<BookingUnitViewModel>();
                foreach (var b in bookingUnits)
                {
                    retList.Add(
                        new BookingUnitViewModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            Address = b.Address + ", " + b.City.Name + ", " + b.City.Country.Name,
                            ImgUrl = b.Pictures.First().Value
                        }
                    );
                }

                return retList;
            }
        }
    }
}