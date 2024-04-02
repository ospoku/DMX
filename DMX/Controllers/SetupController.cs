using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DMX.Data;

namespace ODS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SetupController : Controller
    {
        public readonly XContext prx;
        public SetupController(XContext printcontext)
        {
            prx = printcontext;
        }
    }
}
      
 

    

      
     