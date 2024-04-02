using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class EditPatient:ViewComponent
    {
        public readonly XContext dcx;
        public EditPatient(XContext dContext)
        {
            dcx = dContext;
        }

        public IViewComponentResult Invoke(string MemoId)


        {
           

            Memo memoToEdit = new Memo();
            memoToEdit = (from m in dcx.Memos.Include(m => m.Comments.OrderBy(m=>m.CreatedDate)) where m.MemoId==MemoId select m ).FirstOrDefault();

            EditMemoVM editMemoVM = new EditMemoVM
            {
             
            };
            

            return View(editMemoVM);
        }
    }
}
