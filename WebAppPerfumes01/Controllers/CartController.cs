using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppPerfumes01.Data;
using WebAppPerfumes01.Models;
using WebAppPerfumes01.Models.ViewModels;

namespace WebAppPerfumes01.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }



        // تحديث الكمية
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string actionType)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (actionType == "increase")
                    item.Quantity++;
                else if (actionType == "decrease" && item.Quantity > 1)
                    item.Quantity--;

                // إعادة حساب الإجمالي
                item.Total = item.Quantity * item.Price;
            }
            int totalCount = cart.Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("CartCount", totalCount);
            // حفظ السلة في الجلسة
            HttpContext.Session.SetObject("Cart", cart);
            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = quantity
                });
            }
            int totalCount = cart.Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("CartCount", totalCount);
            HttpContext.Session.SetObject("Cart", cart);

            return Json(new { success = true, cartCount = totalCount });
        }

        public IActionResult ViewCart()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
                return RedirectToAction("Index", "Home");

            var locations = _context.DeliveryAreas.ToList();

            var model = new OrderViewModel
            {
                CartItems = cart,
                Total = cart.Sum(c => c.Price * c.Quantity),
                DeliveryLocations = locations
            };
            var cartQuantity = 1; // or get from session/cart logic
            ViewBag.CartQuantity = cartQuantity;
            return View(model);
        }


        // Checkout page
     /*   public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("ViewCart") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            // var cart = HttpContext.Session.GetObject<List<eComsite.Models.Product>>("ViewCart") ?? new List<eComsite.Models.Product>();
            var cartQuantity = 1; // or get from session/cart logic
            ViewBag.CartQuantity = cartQuantity;

            return View(new OrderViewModel { Quantity = cartQuantity });
            //return View();
        }*/


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
                cart.Remove(item);
            int totalCount = cart.Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("CartCount", totalCount);
            // حفظ السلة في الجلسة
            HttpContext.Session.SetObject("Cart", cart);
            return RedirectToAction("ViewCart");
        }

    }
}
