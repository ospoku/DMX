using System.Diagnostics;
using DMX.Models;
using DMX.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        

        public IActionResult Index()
        {
            
          //var breadcrumbs   = new List<BreadcrumbItem>
          //  {
          //      new BreadcrumbItem{Title="Home", Url="/"},
          //      new BreadcrumbItem{Title="Memos", Url="/ViewMemos"}
          //  };

          //  ViewBag.BreadcrumbItems = breadcrumbs;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult About()
        {
         
            return View();
        }
      
    }
}
