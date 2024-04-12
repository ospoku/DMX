using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using DMX.DataProtection;

namespace DMX.ViewComponents
{
    public class Card(XContext PrintContext) : ViewComponent
    {
        public readonly XContext prx = PrintContext;

        public IViewComponentResult Invoke(string Id)
        {
            var card = prx.Letters.Where(a => a.LetterId == @Encryption.Decrypt(Id) & a.IsDeleted == false).Select(a => new CardVM


            {
        

            }).FirstOrDefault();

            return View(card);
        }
            
    }
}
