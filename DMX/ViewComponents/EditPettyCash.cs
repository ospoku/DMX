using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class EditPettyCash(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke(string Id)


        {
           

     
      var      pettyCashToUpdate = (from p in dcx.PettyCashes where p.PettyCashId==Id select p ).FirstOrDefault();

            EditMemoVM editMemoVM = new()
            {
       

            };
            

            return View(editMemoVM);
        }
    }
}
