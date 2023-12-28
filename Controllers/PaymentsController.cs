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
    public class PaymentsController : Controller
    {
        private readonly DBWONDERLUSTContext _context;

        public PaymentsController(DBWONDERLUSTContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var dBWONDERLUSTContext = _context.Payment.Include(p => p.Package).Include(p => p.PackageBooking).Include(p => p.User);
            return View(await dBWONDERLUSTContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Package)
                .Include(p => p.PackageBooking)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PaymetId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description");
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymetId,PackageBookingId,PackageId,UserId,Amount,PayemtMode,CreatedAt")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", payment.PackageId);
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId", payment.PackageBookingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", payment.UserId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", payment.PackageId);
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId", payment.PackageBookingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", payment.UserId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymetId,PackageBookingId,PackageId,UserId,Amount,PayemtMode,CreatedAt")] Payment payment)
        {
            if (id != payment.PaymetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymetId))
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
            ViewData["PackageId"] = new SelectList(_context.Package, "PackageId", "Description", payment.PackageId);
            ViewData["PackageBookingId"] = new SelectList(_context.PackageBooking, "PackageBookingId", "EmailId", payment.PackageBookingId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", payment.UserId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Package)
                .Include(p => p.PackageBooking)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PaymetId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.PaymetId == id);
        }
    }
}
