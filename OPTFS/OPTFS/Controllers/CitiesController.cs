using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CitiesController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Cities
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            ViewBag.SelectedPage = "cityNavItem";
            var cities = db.City.Include(c => c.Country);
            return View(await cities.ToListAsync());
        }

        // GET: Cities/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SelectedPage = "cityNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var city = await db.City
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.SelectedPage = "cityNavItem";
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,CountryId,Active")] City city)
        {
            ViewBag.SelectedPage = "cityNavItem";
            if (ModelState.IsValid)
            {
                db.Add(city);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", city.CountryId);
            return View(city);
        }

        // GET: Cities/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SelectedPage = "cityNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var city = await db.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", city.CountryId);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryId,Active")] City city)
        {
            ViewBag.SelectedPage = "cityNavItem";
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(city);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", city.CountryId);
            return View(city);
        }

        // GET: Cities/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.SelectedPage = "cityNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var city = await db.City
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.SelectedPage = "cityNavItem";
            var city = await db.City.FindAsync(id);
            if (city != null)
            {
                db.City.Remove(city);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public List<City> GetCities(int CountryId)
        {
            var result=db.City.Where(c=>c.CountryId == CountryId).ToList();
            return result;
        }

        private bool CityExists(int id)
        {
            return db.City.Any(e => e.Id == id);
        }
    }
}
