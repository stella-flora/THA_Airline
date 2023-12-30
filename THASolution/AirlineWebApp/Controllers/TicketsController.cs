using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private  FlightDBRepository _fRepo;
        private  TicketDBRepository _tRepo;
        public TicketsController(FlightDBRepository fRepo, TicketDBRepository tRepo)
        {
            _fRepo = fRepo;
            _tRepo = tRepo;
        
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRetailFlights()
        {
            FlightViewModel flightViewModel = new FlightViewModel();

            //Check Fullybooked
            //var totalSeats = flightViewModel.Rows * flightViewModel.Columns;
            //var bookedSeats = _tRepo.GetTickets().Where(x=>x.Cancelled == false).Count();
            //var availableSeats = totalSeats - bookedSeats
            //if availableSeats = 0, fullyBooked = true
            //add in Where:  || fullyBooked == false 

            var retailFlights = _fRepo.GetFlights()
                .Where(x => x.DepartureDate > DateTime.Now)
                .OrderBy(x => x.DepartureDate).ToList();


            var result = from flight in retailFlights
                         select new FlightViewModel()
                         {
                             Id = flightViewModel.Id,
                             CountryFrom = flightViewModel.CountryFrom,
                             CountryTo = flightViewModel.CountryTo,
                             DepartureDate = flightViewModel.DepartureDate,
                             ArrivalDate = flightViewModel.ArrivalDate,
                             RetailPrice = flightViewModel.WholesalePrice + (flightViewModel.WholesalePrice * flightViewModel.ComissionRate)
                         };

            return View(result);
        }

        [HttpGet]
        public IActionResult BookFlight(){ return View();}

        [HttpPost]
        public IActionResult BookFlight(BookFlightViewModel model)
        {
            var foundTicket = _tRepo.GetTickets().SingleOrDefault(x => x.Passport == model.Passport);
            if (foundTicket == null || model.Cancelled == true)
            {
                var flight = _fRepo.GetFlightById(model.FlightdIdFK);
                   
                if (flight.DepartureDate > DateTime.Now)
                {
                    _tRepo.Book(new Ticket()
                    {
                        Id = model.Id,
                        Row = model.Row,
                        Column = model.Column,
                        FlightdIdFK = model.FlightdIdFK,
                        Passport = model.Passport,
                        PricePaid = flight.WholesalePrice + (flight.WholesalePrice * flight.ComissionRate),
                        Cancelled = model.Cancelled
                    });
                }
            }
            else
            {
                throw new Exception("Ticket is not available or already booked");
            }
                
            return View(model);
        }

        public IActionResult GetTicketHistory()
        {
            TicketViewModel ticketViewModel = new TicketViewModel();

            var list = _tRepo.GetTickets().ToList();

            var result = from t in list
                         select new TicketViewModel()
                         {
                             Id = ticketViewModel.Id,
                             Row = ticketViewModel.Row,
                             Column = ticketViewModel.Column,
                             FlightdIdFK = ticketViewModel.FlightdIdFK,
                             PricePaid = ticketViewModel.PricePaid,
                             Cancelled = ticketViewModel.Cancelled
                         };
            


            return View(result);
        }
    }
}
