using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAppPerfumes01.Data;
using WebAppPerfumes01.Models.ViewModels;
using WebAppPerfumes01.Models;

namespace WebAppPerfumes01.Controllers
{
  
 
        public class OrderController : Controller
        {

            private readonly AppDbContext _context;



            private readonly EmailService _emailService;
            private readonly UserManager<IdentityUser> _userManager;

            public OrderController(UserManager<IdentityUser> userManager, EmailService emailService, AppDbContext context)
            {
                _emailService = emailService;
                _userManager = userManager;
                _context = context;
            }

            [HttpPost]
            public async Task<IActionResult> SubmitOrder(OrderViewModel model)
            {

                var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

          
                Random random = new Random();
                    int userId = random.Next(100000, 999999);
                var order = new Order
                {
                        UserId = userId.ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = model.FirstName + model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Location,
                    PaymentMethod = model.PaymentMethod,
                    Quantity = model.Quantity,
                    TotalAmount = model.TotalPrice,
                    OrderDate = DateTime.Now

                };

                    _context.Orders.Add(order); // Assuming _context is your DbContext instance
                    _context.SaveChanges();



                    // إضافة تفاصيل المنتجات في السلة إلى جدول OrderDetail
                    foreach (var item in cart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,  // ربط تفاصيل المنتج بالطلب
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                           Price = item.Price, // السعر الحالي للمنتج
                        };

                        _context.OrderDetails.Add(orderDetail); // إضافة التفاصيل إلى جدول OrderDetail

                }

                _context.SaveChanges();


                
                if (model.Email != "Not Provided")
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser == null)
                    {
                        var newUser = new IdentityUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            EmailConfirmed = true
                        };

                        var result = await _userManager.CreateAsync(newUser, "Default@123");
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(newUser, "User");
                        }
                    }
                }
                // استرجاع السلة من الجلسة
                // var cart = HttpContext.Session.GetObject<List<CartItem>>("ViewCart") ?? new List<CartItem>();

                // بناء نص تفاصيل المنتجات في السلة
                StringBuilder cartDetails = new StringBuilder();
                foreach (var item in cart)
                {
                    cartDetails.AppendLine($"Product ID: {item.ProductId}");  // إضافة رقم الـ ID للمنتج
                    cartDetails.AppendLine($"Product Name: {item.Name}");
                    cartDetails.AppendLine("------------------------------------");
                }


                string Subject = "New Order Received";
                string Body = $"A new order has been received.\n\n" +
                        $"Name: {model.FirstName.Trim()} {model.LastName.Trim()}\n" +
                        $"Email: {model.Email}\n" +
                        $"Phone: {model.Phone}\n" +
                        $"Address: {model.Location}\n" +
                        $"Quantity: {model.Quantity}\n" +
                        $"Price: {model.TotalPrice}\n" +
                        $"Payment Method: {model.PaymentMethod}\n" +
                                      "Details of products in the cart:\n" +
                     cartDetails.ToString();


                _emailService.SendOrderConfirmationEmail(Subject, Body);
            //  var cart = HttpContext.Session.GetObject<List<CartItem>>("ViewCart") ?? new List<CartItem>();
            HttpContext.Session.SetInt32("CartCount", 0);

            HttpContext.Session.Remove("Cart");
                return View("Success");

            }
            /*   [HttpPost]
               public IActionResult SubmitOrderAfterPayment(OrderViewModel model)
               {
                   // نفس محتوى SubmitOrder
                   return SubmitOrder(model).Result;
               }*/
            [HttpPost]
            public IActionResult SaveOrderData(OrderViewModel order)
            {
                // احفظ البيانات في السيشن مؤقتًا
                HttpContext.Session.SetObject("PendingOrder", order);
                return Ok();
            }
         /*   [HttpGet]
            public async Task<IActionResult> SubmitOrder()
            {
                var model = HttpContext.Session.GetObject<OrderViewModel>("PendingOrder");

                if (model == null)
                    return RedirectToAction("Index", "Home"); // أو صفحة خطأ

                // هنا أكملي عملية الحفظ في قاعدة البيانات مثلاً
                // SaveOrderToDatabase(order);

                // امسحي الجلسة بعد الاستخدام
                HttpContext.Session.Remove("PendingOrder");

                if (!ModelState.IsValid)
                {
                    *//*   var order = new Order
                       {
                           FirstName = model.FirstName,
                           LastName = model.LastName,
                           Email = model.Email, // <-- تخزين الإيميل
                           Phone = model.Phone,
                           Location = model.Location,
                           PaymentMethod = model.PaymentMethod,
                           Quantity = model.Quantity,
                       };

                       _context.Orders.Add(order);
                       _context.SaveChanges();
       *//*

                    return View("Checkout", model);
                }
                if (model.Email != "Not Provided")
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser == null)
                    {
                        var newUser = new IdentityUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            EmailConfirmed = true
                        };

                        var result = await _userManager.CreateAsync(newUser, "Default@123");
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(newUser, "User");
                        }
                    }
                }
                string Subject = "New Order Received";
                string Body = $"A new order has been received.\n\n" +
                        $"Name: {model.FirstName} {model.LastName}\n" +
                        $"Email: {model.Email}\n" +
                        $"Phone: {model.Phone}\n" +
                        $"Address: {model.Location}\n" +
                        $"Quantity: {model.Quantity}\n" +
                        $"TotalPrice: {model.TotalPrice}\n" +
                        $"Payment Method: {model.PaymentMethod}";

                _emailService.SendOrderConfirmationEmail(Subject, Body);
                var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Success"); // إعادة توجيه لصفحة النجاح
            }
*/
           /* public IActionResult Checkout()
            {

                return View();
            }*/
            public ActionResult Success()
            {
                if (HttpContext.Session.GetString("Cart") == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var cart = HttpContext.Session.GetString("Cart");

                if (string.IsNullOrEmpty(cart))
                {
                    return RedirectToAction("Index", "Home"); // لا يوجد جلسة => رجوع للصفحة الرئيسية
                }

                return View();
            }



        }
    }
