using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WONDERLUST_PROJECT_ADMINs.Models;

namespace WONDERLUST_PROJECT_ADMINs.Controllers
{
    public class PackagesController : Controller
    {
        private readonly DBWONDERLUSTContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PackagesController(DBWONDERLUSTContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
            var dBWONDERLUSTContext = _context.Package.Include(p => p.Bus).Include(p => p.Category).Include(p => p.City).Include(p => p.Country).Include(p => p.Hotel).Include(p => p.State);
            return View(await dBWONDERLUSTContext.ToListAsync());
        }

        // GET: Packages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package
                .Include(p => p.Bus)
                .Include(p => p.Category)
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Hotel)
                .Include(p => p.State)
                .FirstOrDefaultAsync(m => m.PackageId == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // GET: Packages/Create
        public IActionResult Create()
        {
            ViewData["BusId"] = new SelectList(_context.Bus, "BusId", "BusName");
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName");
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName");
            ViewData["HotelId"] = new SelectList(_context.Hotel, "HotelId", "HotelName");
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName");
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Package package)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string path = Path.Combine(wwwRootPath + "/images/package/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await package.ImageFile.CopyToAsync(fileStream);
                }
                package.Image = fileName;
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusId"] = new SelectList(_context.Bus, "BusId", "BusName", package.BusId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", package.CategoryId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", package.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", package.CountryId);
            ViewData["HotelId"] = new SelectList(_context.Hotel, "HotelId", "HotelName", package.HotelId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", package.StateId);
            return View(package);
        }

        // GET: Packages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            ViewData["BusId"] = new SelectList(_context.Bus, "BusId", "BusName", package.BusId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", package.CategoryId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", package.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", package.CountryId);
            ViewData["HotelId"] = new SelectList(_context.Hotel, "HotelId", "HotelName", package.HotelId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", package.StateId);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PackageId,PackageName,Amount,FromDate,ToDate,NoOfNights,NoOfDays,Image,Description,CategoryId,CountryId,StateId,CityId,BusId,HotelId,IsBlock,CreatedAt")] Package package)
        {
            if (id != package.PackageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(package.PackageId))
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
            ViewData["BusId"] = new SelectList(_context.Bus, "BusId", "BusName", package.BusId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", package.CategoryId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", package.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", package.CountryId);
            ViewData["HotelId"] = new SelectList(_context.Hotel, "HotelId", "HotelName", package.HotelId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", package.StateId);
            return View(package);
        }

        // GET: Packages/Edit/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            ViewData["BusId"] = new SelectList(_context.Bus, "BusId", "BusName", package.BusId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", package.CategoryId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", package.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", package.CountryId);
            ViewData["HotelId"] = new SelectList(_context.Hotel, "HotelId", "HotelName", package.HotelId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", package.StateId);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [Bind("PackageId,PackageName,Amount,FromDate,ToDate,NoOfNights,NoOfDays,Image,Description,CategoryId,CountryId,StateId,CityId,BusId,HotelId,IsBlock,CreatedAt")] Package package)
        {
            if (id != package.PackageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(package.PackageId))
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
            ViewData["BusId"] = new SelectList(_context.Bus, "BusId", "BusName", package.BusId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", package.CategoryId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", package.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", package.CountryId);
            ViewData["HotelId"] = new SelectList(_context.Hotel, "HotelId", "HotelName", package.HotelId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", package.StateId);
            return View(package);
        }

        private bool PackageExists(int id)
        {
            return _context.Package.Any(e => e.PackageId == id);
        }
    }
}
