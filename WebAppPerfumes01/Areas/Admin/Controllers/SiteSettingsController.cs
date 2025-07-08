using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppPerfumes01.Data;
using WebAppPerfumes01.Models;

namespace WebAppPerfumes01.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteSettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SiteSettingsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Edit()
        {
            var settings = _context.SiteSettings.FirstOrDefault();
            if (settings == null)
            {
                // إذا ما فيه بيانات، أنشئ واحدة بشكل مبدئي
                settings = new SiteSettings
                {
                    Phone = "",
                    Email = "",
                    Address = "",
                    FacebookUrl = "",
                    InstagramUrl = "",
                    WhatsAppUrl = ""
                };
                _context.SiteSettings.Add(settings);
                _context.SaveChanges();
            }

            return View(settings);
        }

        [HttpPost]
        public IActionResult Edit(SiteSettings model)
        {
            var settings = _context.SiteSettings.FirstOrDefault();
            if (settings != null)
            {
                settings.Phone = model.Phone;
                settings.Email = model.Email;
                settings.Address = model.Address;
                settings.FacebookUrl = model.FacebookUrl;
                settings.InstagramUrl = model.InstagramUrl;
                settings.WhatsAppUrl = model.WhatsAppUrl;

                _context.SaveChanges();
            }
            return RedirectToAction("Edit");
        }
    }

}
