using System;
using System.Collections.Generic;
using System.Linq;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class TotalPriceCalculator
    {
        public double CalculateTotalPrice (long bookingUnitId, DateTime dateFrom, DateTime dateTo)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int[] years = GetYears(dateFrom, dateTo);
                List<Tuple<int,int>> yr_mnths = MonthsBetween(dateFrom, dateTo).ToList();
                List<Tuple<int,int,int>> yr_mnth_daysCnt = Year_Month_DaysCnt(dateFrom, dateTo, yr_mnths).ToList();

                List<MonthlyPrices> queryData = null;
                if (years[2] == 0)
                {
                    var year = years[0];
                    queryData = ctx.MonthlyPrices
                        .Include("BookingUnit")
                            .Where(x => x.BookingUnit.Id == bookingUnitId && x.Year == year)
                                .OrderBy(x => x.Month)
                                    .ToList();
                }
                else
                {
                    var year1 = years[0];
                    var year2 = years[1];
                    queryData = ctx.MonthlyPrices
                        .Include("BookingUnit")
                            .Where(x => x.BookingUnit.Id == bookingUnitId && (x.Year == year1 || x.Year == year2))
                                .OrderBy(x => x.Month)
                                    .ToList();
                }

                double retVal = 0;
                foreach (var tuple in yr_mnth_daysCnt)
                {
                    double currentPrice = queryData.FirstOrDefault(x => x.Year == tuple.Item1 && x.Month == tuple.Item2).Amount;
                    retVal += currentPrice * tuple.Item3;
                }

                return retVal;
            }
        }

        public int[] GetYears(DateTime dateFrom, DateTime dateTo)
        {
            int yearFrom = dateFrom.Year;
            int yearTo = dateTo.Year;
            int yearDiff = yearTo - yearFrom;

            return new int[] { yearFrom, yearTo, yearDiff };
        }

        public static IEnumerable<Tuple<int, int>> MonthsBetween(DateTime dateFrom, DateTime dateTo)
        {
            DateTime iterator;
            DateTime limit;

            iterator = new DateTime(dateFrom.Year, dateFrom.Month, 1);
            limit = dateTo;

            while (iterator <= limit)
            {
                yield return Tuple.Create(iterator.Year, iterator.Month);
                iterator = iterator.AddMonths(1);
            }
        }

        public static IEnumerable<Tuple<int,int,int>> Year_Month_DaysCnt(DateTime dateFrom, DateTime dateTo, List<Tuple<int,int>> monthsBethween)
        {
            DateTime iterator;
            DateTime limit;

            iterator = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
            limit = dateTo;

            int caseNo = monthsBethween.Count;

            switch(caseNo)
            {
                case 1:
                    int retVal = (dateTo - dateFrom).Days;
                    yield return Tuple.Create(monthsBethween[0].Item1, monthsBethween[0].Item2, retVal);
                    break;
                case 2:
                    DateTime mediumDate = new DateTime(dateTo.Year, dateTo.Month, 1);
                    int retVal1;
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                            retVal1 = (mediumDate - dateFrom).Days;
                        else
                            retVal1 = (dateTo - mediumDate).Days;
                        yield return Tuple.Create(monthsBethween[i].Item1, monthsBethween[i].Item2, retVal1);
                    }
                    break;
                case 3:
                    DateTime mediumDate1 = new DateTime(monthsBethween[1].Item1, monthsBethween[1].Item2, 1);
                    DateTime mediumDate2 = new DateTime(dateTo.Year, dateTo.Month, 1);
                    int retVal2;
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                            retVal2 = (mediumDate1 - dateFrom).Days;
                        else if(i == 1)
                            retVal2 = (mediumDate2 - mediumDate1).Days;
                        else
                            retVal2 = (dateTo - mediumDate2).Days;
                        yield return Tuple.Create(monthsBethween[i].Item1, monthsBethween[i].Item2, retVal2);
                    }
                    break;
            }
        }
    }
}