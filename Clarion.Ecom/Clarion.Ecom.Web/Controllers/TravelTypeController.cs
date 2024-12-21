using Microsoft.AspNetCore.Mvc;

namespace Clarion.Ecom.Web.Controllers
{
    public class TravelTypeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
          
            return View();
        }

        public IActionResult Edit(long id=0)
        {
            
            return View();
        }

        public IActionResult Details(long id = 0)
        {
           
            return View();
        }


    }
}
