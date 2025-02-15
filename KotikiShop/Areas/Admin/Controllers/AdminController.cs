using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KotikiShop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using KotikiShop.Models;

namespace KotikiShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateCatFamily()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCatFamily(CatFamily obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CatFamily.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cat Family created succsessfully!";
                return RedirectToAction("ManageCatFamilies");
            }
            return View(obj);
        }

        public IActionResult EditCatFamily(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var CatFamilyFromDb = _unitOfWork.CatFamily.GetFirstOrDefault(u => u.Id == id);
            if (CatFamilyFromDb == null)
            {
                return NotFound();
            }

            return View(CatFamilyFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCatFamily(CatFamily obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CatFamily.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cat Family updated succsessfully!";
                return RedirectToAction("ManageCatFamilies");
            }
            return View(obj);
        }

        public IActionResult DeleteCatFamily(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var SpecialityFromDb = _unitOfWork.CatFamily.GetFirstOrDefault(u => u.Id == id);
            if (SpecialityFromDb == null)
            {
                return NotFound();
            }
            return View(SpecialityFromDb);
        }

        // POST
        [HttpPost, ActionName("DeleteCatFamily")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCatFamilyPOST(int? id)
        {
            var obj = _unitOfWork.CatFamily.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CatFamily.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cat Family deleted succsessfully!";
            return RedirectToAction("ManageCatFamilies");
        }

        public IActionResult ManageCatFamilies()
        {
            var CatFamilies = _unitOfWork.CatFamily.GetAll();
            return View(CatFamilies);
        }

        public IActionResult ManageCats()
        {
            var Cats = _unitOfWork.Cat.GetAll();
            return View(Cats);
        }
    }
}
