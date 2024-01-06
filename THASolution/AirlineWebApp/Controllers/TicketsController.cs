using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private  FlightDBRepository _fRepo;
        private  ITicketRepository _tRepo;
        public TicketsController(FlightDBRepository fRepo, ITicketRepository tRepo)
        {
            _fRepo = fRepo;
            _tRepo = tRepo;
        
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RetailFlights()
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
                             Id = flight.Id,
                             CountryFrom = flight.CountryFrom,
                             CountryTo = flight.CountryTo,
                             DepartureDate = flight.DepartureDate,
                             ArrivalDate = flight.ArrivalDate,
                             RetailPrice = flight.WholesalePrice + (flight.WholesalePrice * flight.ComissionRate)
                         };

            return View(result);
        }

        [HttpGet]
        public IActionResult BookFlight()
        {
            BookFlightViewModel model = new BookFlightViewModel();

            model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();

            return View(model);
        
        }

        [HttpPost]
        public IActionResult BookFlight(BookFlightViewModel model, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                var foundTicket = _tRepo.GetTickets().SingleOrDefault(x => x.Passport == model.Passport);
                var foundFlight = _fRepo.GetFlightById(model.FlightdIdFK);
                if (foundTicket == null || model.Cancelled == true)
                {
                    

                    if (foundFlight.DepartureDate > DateTime.Now)
                    {
                        string relativePath = "";

                        if (model.ImageFile != null)
                        {

                            string newPassportName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.ImageFile.FileName);

                            string absolutePath = host.WebRootPath + "\\images\\" + newPassportName;

                            relativePath = "/images/" + newPassportName;

                            try
                            {
                                using (FileStream fs = new FileStream(absolutePath, FileMode.OpenOrCreate))
                                {
                                    model.ImageFile.CopyTo(fs);
                                    fs.Flush();
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                        }



                        _tRepo.Book(new Ticket()
                        {
                            Row = model.Row,
                            Column = model.Column,
                            FlightdIdFK = model.FlightdIdFK,
                            PricePaid = model.PricePaid, //foundFlight.WholesalePrice + (foundFlight.WholesalePrice * foundFlight.ComissionRate), 
                            Cancelled = model.Cancelled,
                            Passport = model.Passport,
                            Image = relativePath
                        });

                        if (relativePath == "")
                        {
                            TempData["message"] = "Passport Image was NOT uploaded, but Booking was successful.";
                        }
                        else
                        {
                            TempData["message"] = "Passport Image was uploaded and Booking  was successful.";
                        }

                        return RedirectToAction("RetailFlights");

                       
                    }
                }
                else
                {
                    TempData["error"] = "Booking failed: Ticket is not available or already booked";
                    model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Booking failed." + ex ;
                model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();
                return View(model);
            }

            model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();
            return View(model);
            
        }

        public IActionResult TicketHistory()
        {
            TicketViewModel ticketViewModel = new TicketViewModel();

            var list = _tRepo.GetTickets().ToList();


            ////Price Paid
            //var flights = _fRepo.GetFlights().ToList();
            ////flight.WholesalePrice + (flight.WholesalePrice * flight.ComissionRate)

            //var pricePaid = from flight in flights
            //             select new TicketViewModel()
            //             {
            //                 PricePaid = flight.WholesalePrice + (flight.WholesalePrice * flight.ComissionRate)
            //             };



            var result = from t in list
                         select new TicketViewModel()
                         {
                             Id = t.Id,
                             Row = t.Row,
                             Column = t.Column,
                             FlightdIdFK = t.FlightdIdFK,
                             PricePaid = t.PricePaid,
                             Cancelled = t.Cancelled,
                             Passport = t.Passport,
                             Image = t.Image
                         };
            


            return View(result);
        }
    }
}
