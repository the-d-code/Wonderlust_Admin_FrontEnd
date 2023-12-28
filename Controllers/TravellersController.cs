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
    public class TravellersController : Controller
    {
        private readonly DBWONDERLUSTContext _context;

        public TravellersController(DBWONDERLUSTContext context)
        {
            _context = context;
        }

        // GET: Travellers
        public async Task<IActionResult> Index()
        {
            var dBWONDERLUSTContext = _context.Travellers.Include(t => t.Package).Include(t => t.PackageBooking).Include(t => t.User);
            return View(await dBWONDERLUSTContext.ToListAsync());
        }

        // GET: Travellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travellers = await _context.Travellers
                .Include(t => t.Package)
                .Include(t => t.PackageBooking)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TravellersId == id);
            if (travellers == null)
            {
                return NotFound();
            }

            return View(travellers);
        }

        // GET: Travellers/Create
        public IActionResult Create()
        {
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description");
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Travellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TravellersId,PackageBookingId,PackageId,UserId,FullName,Age,Gender,AadharCardNo,ContactNo,BloodGroup,Dob,CreatedAt")] Travellers travellers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(travellers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", travellers.PackageId);
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId", travellers.PackageBookingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", travellers.UserId);
            return View(travellers);
        }

        // GET: Travellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travellers = await _context.Travellers.FindAsync(id);
            if (travellers == null)
            {
                return NotFound();
            }
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", travellers.PackageId);
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId", travellers.PackageBookingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", travellers.UserId);
            return View(travellers);
        }

        // POST: Travellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TravellersId,PackageBookingId,PackageId,UserId,FullName,Age,Gender,AadharCardNo,ContactNo,BloodGroup,Dob,CreatedAt")] Travellers travellers)
        {
            if (id != travellers.TravellersId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(travellers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravellersExists(travellers.TravellersId))
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
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", travellers.PackageId);
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId", travellers.PackageBookingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", travellers.UserId);
            return View(travellers);
        }

        // GET: Travellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travellers = await _context.Travellers
                .Include(t => t.Package)
                .Include(t => t.PackageBooking)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TravellersId == id);
            if (travellers == null)
            {
                return NotFound();
            }

            return View(travellers);
        }

        // POST: Travellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var travellers = await _context.Travellers.FindAsync(id);
            _context.Travellers.Remove(travellers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TravellersExists(int id)
        {
            return _context.Travellers.Any(e => e.TravellersId == id);
        }
    }
}
