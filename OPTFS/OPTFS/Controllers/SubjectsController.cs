using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext db;

        public SubjectsController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            ViewBag.SelectedPage = "subjectNavItem";
            var subjects = db.Subject.Include(s => s.Specialty);
            return View(await subjects.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SelectedPage = "subjectNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var subject = await db.Subject
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewBag.SelectedPage = "subjectNavItem";
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name");
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SpecialtyId,Active")] Subject subject)
        {
            ViewBag.SelectedPage = "subjectNavItem";
            if (ModelState.IsValid)
            {
                if (Request.Form.Files?.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string url = @"uploads/" + DateTime.Now.Ticks + "_" + file.FileName.GetHashCode() + System.IO.Path.GetExtension(file.FileName);
                    string newPath = @"wwwroot\" + url.Replace('/', '\\');
                    string dir = Path.GetDirectoryName(newPath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), newPath);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        subject.LogoUrl = url;
                    }
                }
                db.Add(subject);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", subject.SpecialtyId);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SelectedPage = "subjectNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var subject = await db.Subject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", subject.SpecialtyId);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,LogoUrl,SpecialtyId,Active")] Subject subject)
        {
            ViewBag.SelectedPage = "subjectNavItem";
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form.Files?.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        string url = @"uploads/" + DateTime.Now.Ticks + "_" + file.FileName.GetHashCode() + System.IO.Path.GetExtension(file.FileName);
                        string newPath = @"wwwroot\" + url.Replace('/', '\\');
                        string dir = Path.GetDirectoryName(newPath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), newPath);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            subject.LogoUrl = url;
                        }
                    }
                    db.Update(subject);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", subject.SpecialtyId);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.SelectedPage = "subjectNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var subject = await db.Subject
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.SelectedPage = "subjectNavItem";
            var subject = await db.Subject.FindAsync(id);
            if (subject != null)
            {
                db.Subject.Remove(subject);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return db.Subject.Any(e => e.Id == id);
        }
    }
}
