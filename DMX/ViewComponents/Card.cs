using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class Card(XContext PrintContext) : ViewComponent
    {
        public readonly XContext prx = PrintContext;

        public IViewComponentResult Invoke(string Id)
        {
            var card = prx.Documents.Where(a => a.DocumentId == Id & a.IsDeleted == false).Select(a => new CardVM


            {
        

            }).FirstOrDefault();

            return View(card);
        }
            
    }
}
