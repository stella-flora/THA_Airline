using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketDBRepository
    {

        private AirlineDbContext _db;

        public TicketDBRepository()
        {
        }

        public TicketDBRepository(AirlineDbContext db) 
        {
            _db = db;
        }

        public void Book(Ticket t) 
        {
            bool isBooked = _db.Tickets.Any(ticket => ticket.Id == t.Id);
            if (isBooked)
            {
                throw new Exception("Ticket is already booked.");
            }
            else
            {
                _db.Tickets.Add(t);
                _db.SaveChanges();
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
