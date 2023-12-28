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
    public class HotelsController : Controller
    {
        private readonly DBWONDERLUSTContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HotelsController(DBWONDERLUSTContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
            var dBWONDERLUSTContext = _context.Hotel.Include(h => h.City).Include(h => h.Country).Include(h => h.State);
            return View(await dBWONDERLUSTContext.ToListAsync());
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .Include(h => h.City)
                .Include(h => h.Country)
                .Include(h => h.State)
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName");
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId");
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName");
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string path = Path.Combine(wwwRootPath + "/images/hotel/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await hotel.ImageFile.CopyToAsync(fileStream);
                }
                hotel.Image = fileName;
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", hotel.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", hotel.CountryId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", hotel.StateId);
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", hotel.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", hotel.CountryId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", hotel.StateId);
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId,HotelName,CountryId,StateId,CityId,Image,Description,IsActive,CreatedAt")] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (hotel.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var oldPath = string.Empty;
                        if (hotel.Image != null)
                        {
                            oldPath = Path.Combine(wwwRootPath + "/images/hotel/", hotel.Image);
                        }
                        string fileName = Guid.NewGuid().ToString() + ".jpg";
                        string path = Path.Combine(wwwRootPath + "/images", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await hotel.ImageFile.CopyToAsync(fileStream);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }
                        hotel.Image = fileName;
                    }
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
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
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", hotel.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", hotel.CountryId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", hotel.StateId);
            return View(hotel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", hotel.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", hotel.CountryId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", hotel.StateId);
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id,Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (hotel.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var oldPath = string.Empty;
                        if (hotel.Image != null)
                        {
                            oldPath = Path.Combine(wwwRootPath + "/images/hotel/", hotel.Image);
                        }
                        string fileName = Guid.NewGuid().ToString() + ".jpg";
                        string path = Path.Combine(wwwRootPath + "/images", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await hotel.ImageFile.CopyToAsync(fileStream);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }
                        hotel.Image = fileName;
                    }
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
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
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", hotel.CityId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", hotel.CountryId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", hotel.StateId);
            return View(hotel);
        }


        private bool HotelExists(int id)
        {
            return _context.Hotel.Any(e => e.HotelId == id);
        }
    }
}
