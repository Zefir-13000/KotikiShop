using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Models;
using KotikiShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace KotikiShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["CatFamiliesCatalog"] = _unitOfWork.CatFamily.GetAllAsNoTracking().ToList();
            var userId = _userManager.GetUserId(User);
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.ApplicationUserId == userId, includeProperties: "CartItems");
            if (cart != null)
                ViewData["UserCartCount"] = cart.TotalItems;
            base.OnActionExecuting(context);
        }

        [HttpPost]
        public IActionResult AddComment(CatComment catComment)
        {
            if (string.IsNullOrEmpty(catComment.Message))
            {
                TempData["failure"] = "The message was empty!";
                return RedirectToAction("OrderDetails", "Customer", new { area = "Customer", id = catComment.CatId });
            }
            var userId = _userManager.GetUserId(User);
            catComment.UserId = userId;
            _unitOfWork.CatComment.Add(catComment);
            _unitOfWork.Save();
            return RedirectToAction("OrderDetails", "Customer", new { area = "Customer", id = catComment.CatId });
        }

        [HttpPost]
        public IActionResult AddToCart(int catId)
        {
            var userId = _userManager.GetUserId(User);
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.ApplicationUserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };
                _unitOfWork.Cart.Add(cart);
            }

            var cat = _unitOfWork.Cat.GetFirstOrDefault(u => u.Id == catId, includeProperties: "CatFamily");
            if (cat == null) return NotFound();

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == catId);
            if (cartItem != null)
            {
                // Update quantity if the item exists
                cartItem.Quantity += 1;
            }
            else
            {
                // Add a new item to the cart
                cart.CartItems.Add(new CartItem
                {
                    ProductId = catId,
                    Quantity = 1,
                    UnitPrice = cat.Price ?? 0.0001f
                });
            }
            _unitOfWork.Save();

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult UserLike(int catId)
        {
            return StatusCode(200);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            // Get the current user's ID
            var userId = _userManager.GetUserId(User);

            // Find the cart for the logged-in user
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.ApplicationUserId == userId, includeProperties: "CartItems");
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            // Find the cart item to decrease quantity or remove
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            if (cartItem.Quantity > 1)
            {
                // Decrease the quantity
                cartItem.Quantity -= 1;
            }
            else
            {
                // Remove the item if the quantity is 0 or less
                cart.CartItems.Remove(cartItem);
            }

            // Save changes
            _unitOfWork.Save();

            return RedirectToAction("Cart");
        }


        public IActionResult OrderDetails(int? id)
        {
            var cat = _unitOfWork.Cat.GetFirstOrDefault(u => u.Id == id, includeProperties: "CatFamily");

            if (cat == null)
            {
                return NotFound();
            }

            var comments = _unitOfWork.CatComment.GetAll(u => u.CatId == cat.Id, includeProperties: "User").ToList();
            CatCommentVM catCommentVM = new()
            {
                catComments = comments,
                cat = cat
            };

            return View(catCommentVM);
        }

        public IActionResult Cart()
        {
            var userId = _userManager.GetUserId(User);

            // Find the cart for the logged-in user
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.ApplicationUserId == userId, includeProperties: "CartItems,CartItems.Product,CartItems.Product.CatFamily");
            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };
                _unitOfWork.Cart.Add(cart);
            }
            _unitOfWork.Save();
            return View(cart);
        }

        public IActionResult Payment()
        {
            var userId = _userManager.GetUserId(User);
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.ApplicationUserId == userId, includeProperties: "CartItems,CartItems.Product");
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }
            return View(cart);
        }
    }
}

