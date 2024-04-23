using CIS.Data;
using CIS.Models;
using CIS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DMX.ViewComponents
{
    public class AddInternalTraining : ViewComponent
    {

        public AddInternalTraining()
        {



        }

        public IViewComponentResult Invoke()
        {

            AddInternalTrainingVM trainingVM = new AddInternalTrainingVM
            {

            };






            return View(trainingVM);
        }
    }
}

