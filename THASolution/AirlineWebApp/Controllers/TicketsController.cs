using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
