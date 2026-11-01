using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using task1.Models;


namespace task1.Controllers
{
    public class CafeteriaController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Products()
        {
            return View();
        }



        public IActionResult Suppliers()
        {
            return View();
        }



        public IActionResult SupplyStatus()
        {
            return View();
        }



    }
}
