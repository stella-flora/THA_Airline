using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class TicketViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        [DisplayName("Flight ID")]
        public int FlightdIdFK { get; set; }

        [DisplayName("Price Paid")]
        public double PricePaid { get; set; }
        public string Passport { get; set; }
        public string? Image { get; set; }
        
        public bool Cancelled { get; set; }
        

    }
}
