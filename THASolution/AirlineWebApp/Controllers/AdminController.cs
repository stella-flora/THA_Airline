using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        //Create an AdminController.cs, where it allows the admin to
        //select from a list of flights,
        // get a list of tickets,
        // select a ticket
        // and will be able to view info about the ticket
        // including the passport image uploaded. [3.5]

        public IActionResult Index()
        {
            return View();
        }
    }
}
