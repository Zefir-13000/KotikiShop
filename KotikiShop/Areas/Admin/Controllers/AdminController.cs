using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KotikiShop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using KotikiShop.Models;
using KotikiShop.Models.ViewModels;
using KotikiShop.DataAccess.Migrations;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KotikiShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IWebHostEnvironment hostEnviroment, IUnitOfWork unitOfWork)
        {
            _hostEnviroment = hostEnviroment;
            _unitOfWork = unitOfWork;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["CatFamiliesCatalog"] = _unitOfWork.CatFamily.GetAll().ToList();
            base.OnActionExecuting(context);
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

            var CatFamilyFromDb = _unitOfWork.CatFamily.GetFirstOrDefault(u => u.Id == id);
            if (CatFamilyFromDb == null)
            {
                return NotFound();
            }
            return View(CatFamilyFromDb);
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

        public IActionResult CreateCat()
        {
            var catFamilies = _unitOfWork.CatFamily.GetAll();
            CatVM catVM = new CatVM
            {
                catFamilies = catFamilies,
                Name = "",
                Description  = "",
                Birthday = DateOnly.FromDateTime(DateTime.Now),
                Gender = CatGender.NONE
            };
            return View(catVM);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCat([FromForm] CatVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(file.FileName);
                Cat cat = new()
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    Price = obj.Price ?? 0,
                    Birthday = obj.Birthday,
                    Gender = obj.Gender,
                    CatFamilyId = obj.CatFamilyId
                };
                if (file != null)
                {
                    string wwwRootPath = _hostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\cats");
                    var extension = Path.GetExtension(file.FileName);

                    string ImageUrl = @"\images\cats\" + fileName + extension;
                    if (ImageUrl != null && ImageUrl != @"\images\no_picture.png")
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }


                    if (cat != null)
                    {
                        cat.ImageUrl = ImageUrl;
                    }
                }
                else
                {
                    cat.ImageUrl = @"\images\no_picture.png";
                }
                _unitOfWork.Cat.Add(cat);
                _unitOfWork.Save();
                TempData["success"] = "Cat created succsessfully!";
                return RedirectToAction("ManageCats");
            }
            return View(obj);
        }

        public IActionResult EditCat(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var CatFromDb = _unitOfWork.Cat.GetFirstOrDefault(u => u.Id == id);
            if (CatFromDb == null)
            {
                return NotFound();
            }

            var catFamilies = _unitOfWork.CatFamily.GetAll();
            CatVM catVM = new()
            {
                catFamilies = catFamilies,
                Name = CatFromDb.Name,
                Description = CatFromDb.Description,
                Price = CatFromDb.Price,
                Birthday = CatFromDb.Birthday,
                Gender = CatFromDb.Gender,
                ImageUrl = CatFromDb.ImageUrl,
                CatFamilyId = CatFromDb.CatFamilyId
            };
            return View(catVM);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCat(CatVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                Cat cat = new()
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description = obj.Description,
                    Price = obj.Price ?? 0,
                    Birthday = obj.Birthday,
                    Gender = obj.Gender,
                    CatFamilyId = obj.CatFamilyId,
                    ImageUrl = obj.ImageUrl
                };
                var CatFromDb = _unitOfWork.Cat.GetFirstOrDefault(u => u.Id == obj.Id, tracked: false);
                if (file != null)
                {
                    string wwwRootPath = _hostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\cats");
                    var extension = Path.GetExtension(file.FileName);

                    string ImageUrl = @"\images\cats\" + fileName + extension;
                    if (CatFromDb.ImageUrl != null && CatFromDb.ImageUrl != @"\images\no_picture.png")
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, CatFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }


                    if (cat != null)
                    {
                        cat.ImageUrl = ImageUrl;
                    }
                }
                else
                {
                    cat.ImageUrl = CatFromDb.ImageUrl ?? @"\images\no_picture.png";
                }

                _unitOfWork.Cat.Update(cat);
                _unitOfWork.Save();
                TempData["success"] = "Cat updated succsessfully!";
                return RedirectToAction("ManageCats");
            }
            return View(obj);
        }

        public IActionResult DeleteCat(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var CatFromDb = _unitOfWork.Cat.GetFirstOrDefault(u => u.Id == id);
            if (CatFromDb == null)
            {
                return NotFound();
            }
            return View(CatFromDb);
        }

        // POST
        [HttpPost, ActionName("DeleteCat")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCatPOST(int? id)
        {
            var obj = _unitOfWork.Cat.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            string wwwRootPath = _hostEnviroment.WebRootPath;
            if (obj.ImageUrl != null && obj.ImageUrl != @"\images\no_picture.png")
            {
                var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.Cat.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cat deleted succsessfully!";
            return RedirectToAction("ManageCats");
        }

        public IActionResult ManageCats()
        {
            var Cats = _unitOfWork.Cat.GetAll(includeProperties: "CatFamily");
            return View(Cats);
        }
    }
}
