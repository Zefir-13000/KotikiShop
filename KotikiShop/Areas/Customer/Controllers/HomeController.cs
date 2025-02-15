using KotikiShop.Utility;
using Microsoft.AspNetCore.Mvc;

namespace KotikiShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //if (User.IsInRole(SD.Role_User_Admin))
            //{
            //    return RedirectToAction("Index", "Admin", new { area = "Admin" });
            //}
            //else if (User.IsInRole(SD.Role_User_Customer))
            //{
            //    return RedirectToAction("Index", "Customer", new { area = "Customer" });
            //}
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
