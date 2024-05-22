using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class SpecialtiesController : Controller
    {
        private readonly ApplicationDbContext db;

        public SpecialtiesController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Specialties
        public async Task<IActionResult> Index()
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            return View(await db.Specialty.ToListAsync());
        }

        // GET: Specialties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await db.Specialty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // GET: Specialties/Create
        public IActionResult Create()
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            return View();
        }

        // POST: Specialties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Active")] Specialty specialty)
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            if (ModelState.IsValid)
            {
                db.Add(specialty);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await db.Specialty.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST: Specialties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Active")] Specialty specialty)
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            if (id != specialty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(specialty);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialty.Id))
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
            return View(specialty);
        }

        // GET: Specialties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await db.Specialty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.SelectedPage = "specialtyNavItem";
            var specialty = await db.Specialty.FindAsync(id);
            if (specialty != null)
            {
                db.Specialty.Remove(specialty);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyExists(int id)
        {
            return db.Specialty.Any(e => e.Id == id);
        }
    }
}
