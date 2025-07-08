using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebAppPerfumes01.Data;
using WebAppPerfumes01.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace WebAppPerfumes01.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CPanel : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CPanel(SignInManager<IdentityUser> signInManager, AppDbContext context, UserManager<IdentityUser> userManager    )
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {


            var totalUsers = await _userManager.Users.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var newOrdersToday = await _context.Orders
                                    .Where(o => o.OrderDate.Date == DateTime.Today)
                                    .CountAsync();
      

            var model = new DashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalOrders = totalOrders,
                NewOrdersToday = newOrdersToday,
            };

            return View(model);
        }

        public IActionResult UsersList()
        {
            var users = _userManager.Users
                .Where(u => u.Email != "PERFUMESALES2025@gmail.com") // استبعاد هذا الإيميل
                .ToList();

            return View(users);
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