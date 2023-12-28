using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WONDERLUST_PROJECT_ADMINs.Models;

namespace WONDERLUST_PROJECT_ADMINs.Controllers
{
    public class PackageBookingsController : Controller
    {
        private readonly DBWONDERLUSTContext _context;

        public PackageBookingsController(DBWONDERLUSTContext context)
        {
            _context = context;
        }

        // GET: PackageBookings
        public async Task<IActionResult> Index()
        {
            var dBWONDERLUSTContext = _context.PackageBooking.Include(p => p.Package).Include(p => p.User);
            return View(await dBWONDERLUSTContext.ToListAsync());
        }

        // GET: PackageBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageBooking = await _context.PackageBooking
                .Include(p => p.Package)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PackageBookingId == id);
            if (packageBooking == null)
            {
                return NotFound();
            }

            return View(packageBooking);
        }

        // GET: PackageBookings/Create
        public IActionResult Create()
        {
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: PackageBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PackageBookingId,PackageId,UserId,EmailId,ContactNumber,BookingDate,NoOfTravelers,CreatedAt")] PackageBooking packageBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(packageBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", packageBooking.PackageId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", packageBooking.UserId);
            return View(packageBooking);
        }

        // GET: PackageBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageBooking = await _context.PackageBooking.FindAsync(id);
            if (packageBooking == null)
            {
                return NotFound();
            }
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", packageBooking.PackageId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", packageBooking.UserId);
            return View(packageBooking);
        }

        // POST: PackageBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PackageBookingId,PackageId,UserId,EmailId,ContactNumber,BookingDate,NoOfTravelers,CreatedAt")] PackageBooking packageBooking)
        {
            if (id != packageBooking.PackageBookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(packageBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageBookingExists(packageBooking.PackageBookingId))
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
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", packageBooking.PackageId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", packageBooking.UserId);
            return View(packageBooking);
        }

        // GET: PackageBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageBooking = await _context.PackageBooking
                .Include(p => p.Package)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PackageBookingId == id);
            if (packageBooking == null)
            {
                return NotFound();
            }

            return View(packageBooking);
        }

        // POST: PackageBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var packageBooking = await _context.PackageBooking.FindAsync(id);
            _context.PackageBooking.Remove(packageBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackageBookingExists(int id)
        {
            return _context.PackageBooking.Any(e => e.PackageBookingId == id);
        }
    }
}
