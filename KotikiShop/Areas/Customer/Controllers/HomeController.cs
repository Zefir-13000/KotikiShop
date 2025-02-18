using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Models;
using KotikiShop.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace KotikiShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["CatFamiliesCatalog"] = _unitOfWork.CatFamily.GetAllAsNoTracking().ToList();
            var userId = _userManager.GetUserId(User);
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.ApplicationUserId == userId, includeProperties: "CartItems");
            ViewData["UserCartCount"] = cart.TotalItems;
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

        public IActionResult Catalog(string? family, int? age, int? price, string? gender, string? search)
        {
            
            IEnumerable<Cat> cats = _unitOfWork.Cat.GetAll(includeProperties: "CatFamily");

            if (!string.IsNullOrEmpty(search))
            {
                string lowerSearch = search.ToLower();
                cats = cats.Where(u => u.Name.ToLower().Contains(lowerSearch) ||
                                       (!string.IsNullOrEmpty(u.Description) && u.Description.ToLower().Contains(lowerSearch)));
            }

            
            if (!string.IsNullOrEmpty(family))
            {
                string lowerFamily = family.ToLowerInvariant();
                cats = cats.Where(u => u.CatFamily != null && u.CatFamily.Name.ToLowerInvariant() == lowerFamily);
            }

            
            if (age.HasValue)
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                if (age.Value == 1)
                {
                    
                    DateOnly oneYearAgo = today.AddYears(-1);
                    cats = cats.Where(u => u.Birthday > oneYearAgo);
                }
                else
                {
                   
                    DateOnly ageThreshold = today.AddYears(-(age.Value-1));
                    cats = cats.Where(u => u.Birthday <= ageThreshold);
                }
            }

            
            if (price.HasValue && price.Value > 0)
            {
                cats = cats.Where(u => u.Price.HasValue && u.Price.Value <= price.Value);
            }

            
            if (!string.IsNullOrEmpty(gender) && gender.ToLowerInvariant() != "all")
            {
                if (gender.ToLowerInvariant() == "male")
                {
                    cats = cats.Where(u => u.Gender == CatGender.MALE);
                }
                else if (gender.ToLowerInvariant() == "female")
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
