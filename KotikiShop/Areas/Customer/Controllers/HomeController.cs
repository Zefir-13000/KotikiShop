using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Models;
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
            ViewData["CatFamiliesCatalog"] = _unitOfWork.CatFamily.GetAllAsNoTracking().ToList();
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

        public IActionResult Catalog(string? family, int? age, int? price, string? gender)
        {
            var cats = _unitOfWork.Cat.GetAll(includeProperties: "CatFamily");

            if (family != null)
            {
                cats = cats.Where(u => u.CatFamily.Name.ToLower() == family.ToLower());
            }

            if (age.HasValue)
            {
                DateOnly ageThreshold = DateOnly.FromDateTime(DateTime.Today.AddYears(-age.Value));
                cats = cats.Where(u => u.Birthday <= ageThreshold);
            }

            if (price.HasValue)
            {
                cats = cats.Where(u => u.Price.HasValue && u.Price.Value <= price.Value);
            }

            if (!string.IsNullOrEmpty(gender) && gender.ToLower() != "all")
            {
                if (gender.ToLower() == "male")
                {
                    cats = cats.Where(u => u.Gender == CatGender.MALE);
                }
                else if (gender.ToLower() == "female")
                {
                    cats = cats.Where(u => u.Gender == CatGender.FEMALE);
                }
            }

            return View(cats);
        }

        [HttpPost, ActionName("Catalog")]
        [ValidateAntiForgeryToken]
        public IActionResult CatalogPOST(string? family)
        {
            return RedirectToAction("Catalog", family);
        }
    }


}
