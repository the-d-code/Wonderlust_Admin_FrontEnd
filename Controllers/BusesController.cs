using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WONDERLUST_PROJECT_ADMINs.Models;

namespace WONDERLUST_PROJECT_ADMINs.Controllers
{
    public class BusesController : Controller
    {
        private readonly DBWONDERLUSTContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BusesController(DBWONDERLUSTContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        // GET: Buses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bus.ToListAsync());
        }

        // GET: Buses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Bus
                .FirstOrDefaultAsync(m => m.BusId == id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        // GET: Buses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bus bus)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string path = Path.Combine(wwwRootPath + "/images/bus/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await bus.ImageFile.CopyToAsync(fileStream);
                }
                bus.Image = fileName;



                _context.Add(bus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bus);
        }


        // GET: Buses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Bus.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        // POST: Buses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bus bus)
        {
            if (id != bus.BusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (bus.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var oldPath = string.Empty;
                        if (bus.Image != null)
                        {
                            oldPath = Path.Combine(wwwRootPath + "/images/bus/", bus.Image);
                        }
                        string fileName = Guid.NewGuid().ToString() + ".jpg";
                        string path = Path.Combine(wwwRootPath + "/images", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await bus.ImageFile.CopyToAsync(fileStream);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }
                        bus.Image = fileName;
                    }
                    _context.Update(bus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusExists(bus.BusId))
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
            return View(bus);
        }

        // GET: Buses/Edit/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Bus.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        // POST: Buses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Bus bus)
        {
            if (id != bus.BusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (bus.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var oldPath = string.Empty;
                        if (bus.Image != null)
                        {
                            oldPath = Path.Combine(wwwRootPath + "/images/bus", bus.Image);
                        }
                        string fileName = Guid.NewGuid().ToString() + ".jpg";
                        string path = Path.Combine(wwwRootPath + "/images", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await bus.ImageFile.CopyToAsync(fileStream);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }
                        bus.Image = fileName;
                    }
                    _context.Update(bus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusExists(bus.BusId))
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
            return View(bus);
        }

        private bool BusExists(int id)
        {
            return _context.Bus.Any(e => e.BusId == id);
        }
    }
}
