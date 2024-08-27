
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewBreadcrumbs : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<BreadcrumbItem> items)
        {



            return View(items);

        }
    }
        public class BreadcrumbItem
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
    }

