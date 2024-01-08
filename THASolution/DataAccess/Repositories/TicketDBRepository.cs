using DataAccess.Context;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketDBRepository: ITicketRepository
    {

        private AirlineDbContext _db;

        public TicketDBRepository(AirlineDbContext db) 
        {
            _db = db;
        }

        public void Book(Ticket t)
        { 
            var foundTicket = _db.Tickets.SingleOrDefault(x => x.FlightdIdFK == t.FlightdIdFK && x.Row == t.Row && x.Column == t.Column);
            if (foundTicket == null)
            {
                _db.Tickets.Add(t);
                _db.SaveChanges();
                
            }
            else
            {
                throw new Exception("Ticket is already booked.");
            }
        }

        public void Cancel(int ticketId) 
        {
            Ticket ticket = _db.Tickets.SingleOrDefault(t => t.Id == ticketId);
            if (ticket != null)
            {
                ticket.Cancelled = true;
                _db.SaveChanges();
            }
            else
            {
                throw new Exception("Ticket was not found.");
            }
        }

        public IQueryable<Ticket> GetTickets()
        {
            return _db.Tickets;
        
        }
    }
}
