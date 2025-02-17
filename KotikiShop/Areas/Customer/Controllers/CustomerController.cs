using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KotikiShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OrderDetails(int? id)
        {
            Cat cat = _unitOfWork.Cat.GetFirstOrDefault(u => u.Id == id);
            return View(cat);
        }
    }
}

