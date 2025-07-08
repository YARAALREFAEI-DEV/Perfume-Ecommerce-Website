using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPerfumes01.Data;
using WebAppPerfumes01.Models;

namespace WebAppPerfumes01.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DeliveryAreasController : Controller
    {
        private readonly AppDbContext _context;

        public DeliveryAreasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DeliveryAreas
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliveryAreas.ToListAsync());
        }


        // GET: Admin/DeliveryAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DeliveryAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AreaName,DeliveryFee")] DeliveryArea deliveryArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryArea);
        }

        // GET: Admin/DeliveryAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryArea = await _context.DeliveryAreas.FindAsync(id);
            if (deliveryArea == null)
            {
                return NotFound();
            }
            return View(deliveryArea);
        }

        // POST: Admin/DeliveryAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AreaName,DeliveryFee")] DeliveryArea deliveryArea)
        {
            if (id != deliveryArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryAreaExists(deliveryArea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryArea);
        }

        // GET: Admin/DeliveryAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryArea = await _context.DeliveryAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryArea == null)
            {
                return NotFound();
            }

            return View(deliveryArea);
        }

        // POST: Admin/DeliveryAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryArea = await _context.DeliveryAreas.FindAsync(id);
            if (deliveryArea != null)
            {
                _context.DeliveryAreas.Remove(deliveryArea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryAreaExists(int id)
        {
            return _context.DeliveryAreas.Any(e => e.Id == id);
        }
    }
}
