using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class UIGeneratedController : Controller
    {
        [HttpGet]
        public IActionResult SeatingPlan(int flightNo)
        {

            SeatingPlanViewModel model = new SeatingPlanViewModel();
            model.MaxRows = 33;
            model.MaxColumns = 6;
            
            return View(model);
        }

        [HttpPost]
        public IActionResult SeatingPlan(SeatingPlanViewModel model)
        {
            model.MaxRows = 33;
            model.MaxColumns = 6;

            return View(model);
        }

    }
}
