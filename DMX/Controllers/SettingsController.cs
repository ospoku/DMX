using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DMX.Data;

namespace DMX.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController(XContext context) : Controller
    {
        public readonly XContext dcx = context;


        public IActionResult Preferences()
        {

            return ViewComponent("ViewPreferences");
        }
        public IActionResult SystemSetup() 
        { 
            return ViewComponent("SystemSetup"); 
        }
    }
}






