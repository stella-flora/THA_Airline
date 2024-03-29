﻿using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using System.Collections.Generic;

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
        public IActionResult BookFlight(int id)
        {
            BookFlightViewModel model = new BookFlightViewModel();

            //model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();

            model.Flight = _fRepo.GetFlightById(id);


            //var foundFlight = _fRepo.GetFlightById(model.FlightdIdFK);
            model.PricePaid = model.Flight.WholesalePrice + (model.Flight.WholesalePrice * model.Flight.ComissionRate);

            model.FlightdIdFK = id;


            return View(model);
        
        }

        [HttpPost]
        public IActionResult BookFlight(BookFlightViewModel model, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                //if a ticket of a flight with same seat (row + column)
                var foundTicket = _tRepo.GetTickets().SingleOrDefault(x => x.FlightdIdFK == model.FlightdIdFK && x.Row == model.Row && x.Column == model.Column);
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
                            UserEmail = User.Identity.Name,
                            Row = model.Row,
                            Column = model.Column,
                            FlightdIdFK = model.FlightdIdFK,
                            PricePaid = foundFlight.WholesalePrice + (foundFlight.WholesalePrice * foundFlight.ComissionRate), //model.PricePaid,
                            Cancelled = model.Cancelled,
                            Passport = model.Passport,
                            Image = relativePath

                        });

                        if (relativePath == "")
                        {
                            TempData["message"] = "Passport Image was NOT uploaded, but Booking was successful";
                        }
                        else
                        {
                            TempData["message"] = "Passport Image was uploaded and Booking  was successful";
                        }

                        return RedirectToAction("RetailFlights");

                       
                    }
                }
                else
                {
                    TempData["error"] = "Booking failed: Ticket is not available or already booked";
                    //model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Booking failed. " + ex.Message ;
                //model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();
                return View(model);
            }

            //model.Flights = _fRepo.GetFlights().ToList().Where(x => x.DepartureDate > DateTime.Now).OrderBy(x => x.DepartureDate).ToList();
            return View(model);
            
        }

        [Authorize]
        public IActionResult TicketHistory()
        {
            TicketViewModel ticketViewModel = new TicketViewModel();

            //where UserEmail is User.Identity.Name (logged in user)
            var list = _tRepo.GetTickets().Where(x => x.UserEmail == User.Identity.Name).ToList();


            var result = from t in list
                         select new TicketViewModel()
                         {
                             UserEmail = t.UserEmail,
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
