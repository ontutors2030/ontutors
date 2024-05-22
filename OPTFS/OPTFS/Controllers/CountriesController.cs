using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CountriesController(ApplicationDbContext context)
        {
            db = context;            
        }

        [Authorize(Roles ="Admin")]
        // GET: Countries
        public async Task<IActionResult> Index()
        {
            ViewBag.SelectedPage = "countryNavItem";
            return View(await db.Country.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SelectedPage = "countryNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var country = await db.Country
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [Authorize(Roles = "Admin")]
        // GET: Countries/Create
        public IActionResult Create()
        {
            ViewBag.SelectedPage = "countryNavItem";
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,Active")] Country country)
        {
            ViewBag.SelectedPage = "countryNavItem";
            if (ModelState.IsValid)
            {
                db.Add(country);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        [Authorize(Roles = "Admin")]
        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SelectedPage = "countryNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var country = await db.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Active")] Country country)
        {
            ViewBag.SelectedPage = "countryNavItem";
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(country);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
            return View(country);
        }

        [Authorize(Roles = "Admin")]
        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.SelectedPage = "countryNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var country = await db.Country
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.SelectedPage = "countryNavItem";
            var country = await db.Country.FindAsync(id);
            if (country != null)
            {
                db.Country.Remove(country);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return db.Country.Any(e => e.Id == id);
        }
    }
}
