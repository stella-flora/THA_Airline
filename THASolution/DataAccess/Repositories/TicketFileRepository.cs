using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketFileRepository : ITicketRepository
    {

        private string _path;
        public TicketFileRepository(string path)
        {
            //file path
            _path = path;

            //validation
            if (System.IO.File.Exists(path) == false)
            {
                using (var myFile = System.IO.File.Create(path))
                {
                    myFile.Close();
                }
            }


        }

        public void Book(Ticket t)
        {
            var list = GetTickets().ToList();
            t.Id = list.Count + 1;
            var foundTicket = list.SingleOrDefault(x => x.FlightdIdFK == t.FlightdIdFK && x.Row == t.Row && x.Column == t.Column);
            if (foundTicket == null)
            {
                list.Add(t);
                string ticketsJsonString = JsonSerializer.Serialize(list);
                System.IO.File.WriteAllText(_path, ticketsJsonString);

            }
            else
            {
                throw new Exception("Ticket is already booked.");
            }
        }

        public void Cancel(int ticketId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Ticket> GetTickets()
        {
            string ticketsAsJson = "";
            using (var myFile = System.IO.File.OpenText(_path))
            {
                ticketsAsJson = myFile.ReadToEnd();
                myFile.Close(); 

            }

            if (string.IsNullOrEmpty(ticketsAsJson))
            {
                return new List<Ticket>().AsQueryable();
            }
            else
            {
                //Deserialization: json string into a List<Ticket>
                List<Ticket> Tickets = JsonSerializer.Deserialize<List<Ticket>>(ticketsAsJson);
                return Tickets.AsQueryable();
            }
        }
    }
}
