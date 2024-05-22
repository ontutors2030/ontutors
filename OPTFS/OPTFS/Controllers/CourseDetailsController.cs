using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class CourseDetailsController : Controller
    {
        private readonly ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;
        public CourseDetailsController(UserManager<ApplicationUser> _userManager,
            ApplicationDbContext context)
        {
            db = context;
            this.userManager = _userManager;
        }

        // GET: CourseDetails
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Index(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.SelectedPage = "courseNavItem";
            ViewBag.CourseId = id;
            var courseDetail = db.CourseDetail.Where(c => c.Course.TutorId == userId && 
            (id == null || c.CourseId==id)).Include(c => c.Course);
            return View(await courseDetail.ToListAsync());
        }

        // GET: CourseDetails/Details/5
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var courseDetail = await db.CourseDetail
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseDetail == null)
            {
                return NotFound();
            }
            ViewBag.CourseId = courseDetail.CourseId;
            return View(courseDetail);
        }

        // GET: CourseDetails/Create
        [Authorize(Roles = "Tutor")]
        public IActionResult Create(int?id)
        {
            ViewBag.SelectedPage = "courseNavItem";            
            ViewBag.CourseId = id;
            return View();
        }

        // POST: CourseDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Tutor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CourseId,LevelIndex")] CourseDetail courseDetail)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (ModelState.IsValid)
            {
                db.Add(courseDetail);
                await db.SaveChangesAsync();
                return Redirect("../Index/"+courseDetail.CourseId);
            }
            
            ViewBag.CourseId = courseDetail.CourseId;
            return View(courseDetail);
        }

        [Authorize(Roles = "Tutor")]
        // GET: CourseDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var courseDetail = await db.CourseDetail.FindAsync(id);
            if (courseDetail == null)
            {
                return NotFound();
            }            
            ViewBag.CourseId = courseDetail.CourseId;
            return View(courseDetail);
        }

        // POST: CourseDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CourseId,LevelIndex")] CourseDetail courseDetail)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id != courseDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(courseDetail);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseDetailExists(courseDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("../Index/" + courseDetail.CourseId);
            }
            ViewBag.CourseId = courseDetail.CourseId;
            return View(courseDetail);
        }

        // GET: CourseDetails/Delete/5
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var courseDetail = await db.CourseDetail
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseDetail == null)
            {
                return NotFound();
            }
            ViewBag.CourseId = courseDetail.CourseId;
            return View(courseDetail);
        }

        [Authorize(Roles = "Tutor")]
        // POST: CourseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            var courseDetail = await db.CourseDetail.FindAsync(id);
            if (courseDetail != null)
            {
                ViewBag.CourseId = courseDetail.CourseId;
                db.CourseDetail.Remove(courseDetail);
            }

            await db.SaveChangesAsync();
            return Redirect("../Index/" + ViewBag.CourseId);
        }

        private bool CourseDetailExists(int id)
        {
            return db.CourseDetail.Any(e => e.Id == id);
        }
    }
}
