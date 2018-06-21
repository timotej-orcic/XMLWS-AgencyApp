namespace XML_WS_AgencyApp.Models_DTO
{
    public class Reservation_DTO
    {
        public long BookingUnitMainServerId { get; set; }

        public string ReserveeFirstName { get; set; }

        public string ReserveeLastName { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }
    }
}