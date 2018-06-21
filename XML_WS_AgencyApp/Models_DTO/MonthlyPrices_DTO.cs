namespace XML_WS_AgencyApp.Models_DTO
{
    public class MonthlyPrices_DTO
    {
        public long BookingUnitMainServerId { get; set; }

        public int Year { get; set; }

        public double[] MonthlyPrices { get; set; }
    }
}