using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AddAttendance:ViewComponent
    {
        public IViewComponentResult Invoke(string Id)
        {
            AddAttendanceVM addAttendanceVM = new()
            {

            };

            return View();
        }
    }
}
