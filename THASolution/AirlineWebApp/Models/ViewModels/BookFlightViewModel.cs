using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class BookFlightViewModel
    {
        //public List<Flight> Flights { get; set; }

        [Required]
        [DisplayName("Row")]
        [Range(1, 33, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Row { get; set; }

        [Required]
        [DisplayName("Column")]
        [Range(1, 6, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Column { get; set; }

        [Required]
        [DisplayName("Flight ID")]
        public int FlightdIdFK { get; set; }

        public Flight Flight { get; set; }

        [Required]
        [DisplayName("Passport")]
        public string Passport { get; set; }

        [DisplayName("Price")]
        public double PricePaid { get; set; }

        [DisplayName("Cancelled")]
        public bool Cancelled { get; set; }

        [DisplayName("Passport Image")]
        public string? Image { get; set; }

        public IFormFile ImageFile { get; set; }

        [DisplayName("User Email")]
        public string? UserEmail { get; set; }

    }
}
