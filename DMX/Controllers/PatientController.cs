using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class PatientController : Controller
    {
       
        [HttpGet]
        public IActionResult EditPatient(string Id)
        {
            return ViewComponent("EditPatient", Id);
        }
        public IActionResult ViewPatients()
        {
            return ViewComponent("ViewPatients");
        }
    }
}
