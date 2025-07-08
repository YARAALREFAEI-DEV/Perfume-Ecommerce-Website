using Microsoft.AspNetCore.Mvc;
using WebAppPerfumes01.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAppPerfumes01.Models.ViewComponents
{
    public class AnnouncementsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public AnnouncementsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var activeAnnouncements = await _context.Announcements
                .Where(a => a.IsActive)
                .ToListAsync();

            return View("_AnnouncementPartial", activeAnnouncements);
        }
    }
}
