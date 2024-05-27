using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class EditMaternityLeave:ViewComponent
    {
        public readonly XContext dcx;
        public EditMaternityLeave(XContext dContext)
        {
            dcx = dContext;
        }

        public IViewComponentResult Invoke(string Id)


        {
           

            MaternityLeave maternityToUpdate = new MaternityLeave();
            maternityToUpdate = (from m in dcx.MaternityLeaves where m.MaternityLeaveId==Id select m ).FirstOrDefault();

            EditMaternityLeaveVM editMemoVM = new EditMaternityLeaveVM
            {


            };
            

            return View(editMemoVM);
        }
    }
}
