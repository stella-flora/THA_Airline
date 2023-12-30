using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int FlightdIdFK { get; set; }
        public string Passport { get; set; }
        public double PricePaid { get; set; }
        public bool Cancelled { get; set; }
    }
}
