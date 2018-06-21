using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Models_DTO
{
    public class ReservationStatus_DTO
    {
        public long ReservationMainServerId { get; set; }

        public ReservationStatus ReservationStatus { get; set; }
    }
}