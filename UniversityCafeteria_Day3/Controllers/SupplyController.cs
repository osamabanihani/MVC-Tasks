using Microsoft.AspNetCore.Mvc;

namespace UniversityCafeteria.Controllers
{
    public class SupplyController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Categories = new string[]
            {
                "Drinks", "Food", "Snacks", "Hot Drinks", "Cold Drinks"
            };
            return View();
        }

        public IActionResult Products()
        {
            ViewData["Title"] = "Cafeteria Products";
            ViewData["Manager"] = "Ahmad Ali";
            ViewData["Count"] = 5;
            ViewData["Products"] = new string[]
            {
                "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate"
            };
            return View();
        }

        public IActionResult Suppliers()
        {
            ViewBag.Title = "Our Suppliers";
            ViewBag.Count = 3;
            ViewBag.Suppliers = new string[]
            {
                "Fresh Food Company", "Happy Drinks Company", "Daily Supplies Company"
            };
            return View();
        }

        [HttpGet]
        public IActionResult CreateRequest() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitRequest()
        {
            string employee = Request.Form["EmployeeName"].ToString();
            string product = Request.Form["ProductName"].ToString();
            string quantityText = Request.Form["Quantity"].ToString();
            string date = Request.Form["RequiredDate"].ToString();

            if (string.IsNullOrWhiteSpace(employee))
            {
                ViewBag.Error = "Employee name is required!";
                return View();
            }
            if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
            {
                ViewBag.Error = "Enter a valid quantity!";
                return View();
            }
            if (string.IsNullOrWhiteSpace(date) || !DateTime.TryParse(date, out _))
            {
                ViewBag.Error = "Choose a valid required date!";
                return View();
            }

            TempData["Message"] = $"Request submitted successfully by {employee} for {quantity} {product}. Required date: {date}.";
            return RedirectToAction("About");
        }

        public IActionResult About() => View();

        public IActionResult CheckQuantity(int quantity)
        {
            if (quantity < 20) return Content("Small Order");
            if (quantity <= 50) return Content("Medium Order");
            return Content("Large Order");
        }

        public IActionResult Error() => Content("Something went wrong. Please try again.");
    }
}
