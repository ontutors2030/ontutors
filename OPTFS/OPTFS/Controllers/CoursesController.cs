using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public CoursesController(UserManager<ApplicationUser> _userManager, 
            ApplicationDbContext context)
        {
            db = context;
            this.userManager = _userManager;
        }

        // GET: Courses
        [Authorize(Roles ="Tutor")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.SelectedPage = "courseNavItem";
            var courses = db.Course.Where(c=>c.TutorId== userId).Include(c => c.Subject).Include(c => c.Tutor);
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Course
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Create()
        {
            ViewBag.SelectedPage = "courseNavItem";            
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);
            var Subjects = db.Subject.Where(c => c.SpecialtyId==1 || c.SpecialtyId == applicationUser.SpecialtyId).ToList();
            ViewData["SubjectId"] = new SelectList(Subjects, "Id", "Name");            
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SubjectId,FromDate,ToDate,Sat,Sun,Mon,Tue,Wen,Thi,Fri,FromTime,ToTime,Active,Price,Discount")] Course course)
        {
            ViewBag.SelectedPage = "courseNavItem";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            course.TutorId = userId;
            if (ModelState.IsValid)
            {
                if (Request.Form.Files?.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string url = @"uploads/" + DateTime.Now.Ticks + "_" + file.FileName.GetHashCode() + System.IO.Path.GetExtension(file.FileName);
                    string newPath = @"wwwroot\" + url.Replace('/', '\\');
                    string dir = System.IO.Path.GetDirectoryName(newPath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), newPath);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        course.LogoUrl = url;
                    }
                }
                db.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }            
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);
            var Subjects = db.Subject.Where(c => c.SpecialtyId == 1 || c.SpecialtyId == applicationUser.SpecialtyId).ToList();
            ViewData["SubjectId"] = new SelectList(Subjects, "Id", "Name", course.SubjectId);            
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);
            var Subjects = db.Subject.Where(c => c.SpecialtyId == 1 || c.SpecialtyId == applicationUser.SpecialtyId).ToList();
            ViewData["SubjectId"] = new SelectList(Subjects, "Id", "Name", course.SubjectId);            
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,LogoUrl,SubjectId,FromDate,ToDate,Sat,Sun,Mon,Tue,Wen,Thi,Fri,FromTime,ToTime,Active,Price,Discount")] Course course)
        {
            ViewBag.SelectedPage = "courseNavItem";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            course.TutorId = userId;
            if (id != course.Id)
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
                        string dir = System.IO.Path.GetDirectoryName(newPath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), newPath);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            course.LogoUrl = url;
                        }
                    }
                    db.Update(course);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);
            var Subjects = db.Subject.Where(c => c.SpecialtyId == 1 || c.SpecialtyId == applicationUser.SpecialtyId).ToList();
            ViewData["SubjectId"] = new SelectList(Subjects, "Id", "Name", course.SubjectId);
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Course
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            var course = db.Course
                .Include(c => c.StudentCourses)
                .Include(c => c.CourseDetails)
                .Include(c => c.StudentRequests)
                .Where(c=>c.Id==id)?.FirstOrDefault();

            if (course?.StudentCourses != null)
            {
                foreach (var item in course.StudentCourses)
                {
                    db.StudentCourses.Remove(item);
                }
            }

            if (course?.CourseDetails != null)
            {
                foreach (var item in course.CourseDetails)
                {
                    db.CourseDetail.Remove(item);
                }
            }

            if(course?.StudentRequests != null)
            {
                foreach (var item in course.StudentRequests)
                {
                    db.StudentRequest.Remove(item);
                }
            }

            if (course != null)
            {
                db.Course.Remove(course);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return db.Course.Any(e => e.Id == id);
        }
    }
}
