using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class BookFlightViewModel
    {
        public List<Flight> Flights { get; set; }

        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Row")]
        public int Row { get; set; }

        [DisplayName("Column")]
        public int Column { get; set; }

        [DisplayName("Flight ID")]
        public int FlightdIdFK { get; set; }

        [DisplayName("Passport")]
        public string Passport { get; set; }

        [DisplayName("Price Paid")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price has to be a positive value")]
        public double PricePaid { get; set; }

        [DisplayName("Cancelled")]
        public bool Cancelled { get; set; }

        [DisplayName("Passport Image")]
        public string? Image { get; set; }

        public IFormFile ImageFile { get; set; }

    }
}
