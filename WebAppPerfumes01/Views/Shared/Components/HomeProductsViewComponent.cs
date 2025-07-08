 using Microsoft.AspNetCore.Mvc;
    using WebAppPerfumes01.Data;
    using System.Linq;
namespace WebAppPerfumes01.Views.Shared.Components
{
   

    public class HomeProductsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HomeProductsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var products = _context.Products
     .Where(p => p.Category == "Home Perfumes")
     .OrderBy(p => p.Name) // ← الترتيب
     .Take(6)
     .ToList();

            return View(products);
        }
    }

}
