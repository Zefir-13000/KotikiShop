using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace KotikiShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["CatFamiliesCatalog"] = _unitOfWork.CatFamily.GetAll().ToList();
            base.OnActionExecuting(context);
        }
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

        public IActionResult Catalog(string? family)
        {
            return View(_unitOfWork.Cat.GetAll(includeProperties: "CatFamily"));
        }
    }
}
