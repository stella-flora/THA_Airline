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
        public TicketDBRepository(AirlineDbContext db) 
        {
            _db = db;
        }

        public void Book(Ticket t) 
        {
            //if ticket.id not booked
            // ? = if ticket.id does not exist in gettickets()
            //then add ticket
            //
            //else
            // seat already booked
        }

        public void Cancel(Ticket t) 
        { 
            Ticket myTicket = t;
            //where id = myTicket.id
            //check if ticket.id is in gettickets()
            //then myTicket.cancelled = true;
            //else ticket not found


        }

        public IQueryable<Ticket> GetTickets()
        {
            return _db.Tickets;
        
        }
    }
}
