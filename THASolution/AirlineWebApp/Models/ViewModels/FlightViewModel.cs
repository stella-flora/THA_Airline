using System.ComponentModel;

namespace Presentation.Models.ViewModels
{
    public class FlightViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Departure Date")]
        public DateTime DepartureDate { get; set; }

        [DisplayName("Arrival Date")]
        public DateTime ArrivalDate { get; set; }

        [DisplayName("Country From")]
        public string CountryFrom { get; set; }

        [DisplayName("Country To")]
        public string CountryTo { get; set; }

        [DisplayName(" Price")]
        public double RetailPrice { get; set; }
    }
}
