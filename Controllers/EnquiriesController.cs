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
    public class EnquiriesController : Controller
    {
        private readonly DBWONDERLUSTContext _context;

        public EnquiriesController(DBWONDERLUSTContext context)
        {
            _context = context;
        }

        // GET: Enquiries
        public async Task<IActionResult> Index()
        {
            var dBWONDERLUSTContext = _context.Enquiry.Include(e => e.User);
            return View(await dBWONDERLUSTContext.ToListAsync());
        }

        // GET: Enquiries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiry
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EnquiryId == id);
            if (enquiry == null)
            {
                return NotFound();
            }

            return View(enquiry);
        }

        // GET: Enquiries/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Enquiries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnquiryId,UserId,ContactNumber,EmailId,Message,IsBlock,CreatedAt")] Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enquiry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", enquiry.UserId);
            return View(enquiry);
        }

        // GET: Enquiries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiry.FindAsync(id);
            if (enquiry == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", enquiry.UserId);
            return View(enquiry);
        }

        // POST: Enquiries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnquiryId,UserId,ContactNumber,EmailId,Message,IsBlock,CreatedAt")] Enquiry enquiry)
        {
            if (id != enquiry.EnquiryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enquiry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnquiryExists(enquiry.EnquiryId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", enquiry.UserId);
            return View(enquiry);
        }

        // GET: Enquiries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiry
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EnquiryId == id);
            if (enquiry == null)
            {
                return NotFound();
            }

            return View(enquiry);
        }

        // POST: Enquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enquiry = await _context.Enquiry.FindAsync(id);
            _context.Enquiry.Remove(enquiry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnquiryExists(int id)
        {
            return _context.Enquiry.Any(e => e.EnquiryId == id);
        }
    }
}
