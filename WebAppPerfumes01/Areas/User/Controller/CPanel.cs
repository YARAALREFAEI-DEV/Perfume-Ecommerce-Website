using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebAppPerfumes01.Data;
using WebAppPerfumes01.Models.ViewModels;

namespace WebAppPerfumes01.Areas.User.Controllers
{
    [Area("User")]
    public class CPanel : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public CPanel(SignInManager<IdentityUser> signInManager, AppDbContext context, UserManager<IdentityUser> userManager        )
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string email)
        {
            // If session is not available, redirect to login
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            if (string.IsNullOrEmpty(email))
            {
                // fallback: get from session
                email = HttpContext.Session.GetString("UserEmail");
            }

            // Get orders from database
            var orders = await _context.Orders
                .Where(o => o.Email == email)
                .ToListAsync();

            var viewModel = new UserDashboardViewModel
            {
                UserEmail = email,
                Orders = orders
            };

            return View(viewModel);
        }
        public IActionResult OrderDetails(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            // جلب بيانات الاسم من جدول User (اللي مخزن فيه الاسم)
            var user = _context.users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // جلب بيانات الهاتف والعنوان من آخر أوردر بنفس الإيميل
            var order = _context.Orders
                        .Where(o => o.Email == email)
                        .OrderByDescending(o => o.OrderDate)
                        .FirstOrDefault();

            var model = new UserProfileViewModel
            {
                fName = user.Name.Split(' ').FirstOrDefault() ?? "",
                lName = user.Name.Split(' ').Skip(1).FirstOrDefault() ?? "",
                Email = user.Email,
                PhoneNumber = order?.Phone ?? "",
                Address = order?.Address ?? ""
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult UpdateProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            // تحديث بيانات الهاتف والعنوان في الطلبات - مثلا تحديث آخر طلب فقط
            var order = _context.Orders
                        .Where(o => o.Email == model.Email)
                        .OrderByDescending(o => o.OrderDate)
                        .FirstOrDefault();

            if (order != null)
            {
                order.Phone = model.PhoneNumber;
                order.Address = model.Address;
                _context.SaveChanges();
            }

            // البيانات الاسم والإيميل لا يتم تعديلها هنا لأنها في جدول User فقط

            return RedirectToAction("Profile");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();




            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}