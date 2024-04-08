using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class SickReportController : Controller
    {
        [HttpGet]
        public IActionResult AddSickReport() => ViewComponent("AddSickReport");

        [HttpGet]
        public IActionResult ViewSickReports() => ViewComponent("ViewSickReports");

    }
}
