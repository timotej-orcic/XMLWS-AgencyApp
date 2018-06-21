using System.Collections.Generic;

namespace XML_WS_AgencyApp.Models_DTO
{
    public class BookingUnit_DTO
    {
        public string Name { get; set; }

        public long CityMainServerId { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int PeopleNo { get; set; }

        public long AgentMainServerId { get; set; }

        public long AccomodationTypeMainServerId { get; set; }

        public long AccomodationCategoryMainServerId { get; set; }

        public List<long> BonusFeaturesMainServerIds { get; set; }

        public List<string> Base64ImagesList { get; set; }
    }
}