using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OPTFS.Data;
using OPTFS.Models;
using OPTFS.Models.JsonModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace OPTFS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(ILogger<HomeController> logger,
           RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ApplicationDbContext context)
        {
            this._logger = logger;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = context;
            Initialize();
        }

        public async Task<bool> Initialize()
        {
            if (!roleManager.RoleExistsAsync("Super").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Id = "Super",
                    Name = "Super"
                };
                var x = roleManager.CreateAsync(role).Result;

                ApplicationUser super = new ApplicationUser
                {
                    UserName = SharedStatics.SuperEmail,
                    Email = SharedStatics.SuperEmail,
                    EmailConfirmed = true,
                    UserTypeId = "Super"
                };

                var x1 = userManager.CreateAsync(super, SharedStatics.SuperPass).Result;
                var y1 = userManager.AddToRoleAsync(super, super.UserTypeId).Result;
            }

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Id = "Admin",
                    Name = "Admin"
                };
                var x = roleManager.CreateAsync(role).Result;

                ApplicationUser admin = new ApplicationUser
                {
                    UserName = SharedStatics.AdminEmail,
                    Email = SharedStatics.AdminEmail,
                    EmailConfirmed = true,
                    UserTypeId = "Admin"
                };

                var x1 = userManager.CreateAsync(admin, SharedStatics.AdminPass).Result;
                var y1 = userManager.AddToRoleAsync(admin, admin.UserTypeId).Result;
            }

            if (!roleManager.RoleExistsAsync("Tutor").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Id = "Tutor",
                    Name = "Tutor"
                };
                var x = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Student").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Id = "Student",
                    Name = "Student"
                };
                var x = roleManager.CreateAsync(role).Result;
            }

            if (db.Users.Count()!= 0)
            {
                var users = userManager.Users.Where(u => u.Email == SharedStatics.AdminEmail).ToList();
                if (users != null && users.Count() > 0)
                {
                    foreach (var user in users)
                    {
                        if (!user.EmailConfirmed)
                        {
                            user.EmailConfirmed = true;
                            await userManager.UpdateAsync(user);
                        }
                    }
                }
            }
            await db.SaveChangesAsync();
            return true;
        }

        public bool DeleteUsers()
        {
            if (db.Users.Count() == 0)
            {
                var uList = userManager.Users.ToList();
                foreach (var user in uList)
                {
                    db.Users.Remove(user);
                }
            }

            return true;
        }

        public IActionResult Index()
        {
            ViewBag.SelectedPage = "homeNavItem";
            return View();
        }    

        public IActionResult AboutUS()
        {
            ViewBag.SelectedPage = "aboutUSNavItem";
            return View();
        }

        public IActionResult ContactUS()
        {
            ViewBag.SelectedPage = "contactUSNavItem";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(Contact contact)
        {
            string body = contact.Message;
            if (!string.IsNullOrEmpty(contact.Email))
            {
                body = "Email from:<a href='mailto:" + contact.Email + "'>" + contact.Email + "</a><br/>"+body;
            }

            if (!string.IsNullOrEmpty(contact.PhoneNumber))
            {
                body = "Email from:<a href='tel:" + contact.PhoneNumber + "'>" + contact.PhoneNumber + "</a><br/>" + body;
            }
            await OPTFS.EmailSender.Send("Important Email",body, "ontutors2030@gmail.com", true);
            return RedirectToAction("ContactUS");
        }

        public IActionResult Privacy()
        {
            ViewBag.SelectedPage = "homeNavItem";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.SelectedPage = "homeNavItem";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        

        [Authorize(Roles = "Student")]
        [ActionName("CourseSearch")]
        public async Task<IActionResult> CourseSearch(string q)
        {
            ViewBag.SelectedPage = "homeNavItem";
            ViewBag.q = q;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);
            var courses = db.Course
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                //.Where(c => c.Subject.SpecialtyId == stuentUser.SpecialtyId)
                .Where(c=> c.Name.Contains(q) || c.Subject.Name.Contains(q) || c.Tutor.FullName.Contains(q)||
                 c.Description.Contains(q)|| c.Subject.Description.Contains(q))
                .ToList();

            /*ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", applicationUser.SpecialtyId);
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", applicationUser.CountryId);
            ViewData["CityId"] = new SelectList(db.City, "Id", "Name", applicationUser.CityId);*/
            return View(courses);
        }

        [Authorize(Roles = "Student")]
        [ActionName("CourseSearchResult")]
        public async Task<IActionResult> CourseSearchResult()
        {
            //ViewBag.SelectedPage = "homeNavItem";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);
            var courses = db.Course
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .Where(c => c.Subject.SpecialtyId == stuentUser.SpecialtyId)                
                .ToList();

            /*ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", applicationUser.SpecialtyId);
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", applicationUser.CountryId);
            ViewData["CityId"] = new SelectList(db.City, "Id", "Name", applicationUser.CityId);*/
            return PartialView(courses);
        }

        /*[Authorize(Roles = "Student")]
        [ActionName("CoursePartial")]
        public async Task<IActionResult> CoursePartial(int courseId)
        {
            //ViewBag.SelectedPage = "homeNavItem";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);
            var courses = db.Course
                .Include(c => c.Subject)
                .Include(c => c.Tutor)
                .Where(c => c.Id==courseId)
                .FirstOrDefault();            
            return PartialView(courses);
        }*/

        [Authorize(Roles = "Student")]
        [ActionName("CoursePartial")]
        public async Task<IActionResult> CoursePartial(Course course)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);            
            return PartialView(course);
        }

        [Authorize(Roles = "Student")]
        [ActionName("CourseListPartial")]
        public async Task<IActionResult> CourseListPartial(List<Course> courses)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);            
            return PartialView(courses);
        }

        [Authorize]
        [HttpGet]
        [ActionName("CourseReviewers")]
        public IActionResult CourseReviewers(int id)//CourseId
        {
            ViewBag.CourseId = id;
            var Result = db.Course//.Include(c => c.StudentCourses)
                .Include(c=>c.Tutor)
                .Where(c => c.Id == id).FirstOrDefault();
            return View(Result);
        }

        [Authorize(Roles ="Student")]
        [HttpPost]
        [ActionName("CreateReview")]
        public async Task<IActionResult> CreateReview(StudentCourse newCourse)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);
            var course = db?.Course.Find(newCourse.CourseId);
            if(course != null)
            {
                var oldStudentCourse = db.StudentCourses?
                .Where(c => c.StudentId == userId && c.CourseId == newCourse.CourseId)?.FirstOrDefault();
                if (oldStudentCourse != null)
                {                    
                    oldStudentCourse.CourseId = newCourse.CourseId;
                    oldStudentCourse.StudentId = userId;
                    oldStudentCourse.Rating = newCourse.Rating;
                    oldStudentCourse.ReviewText = newCourse.ReviewText;
                    oldStudentCourse.ReviewDate = DateTime.Now;
                    oldStudentCourse.IsReviewed = true;

                    course.TotalReviews += 1;
                    course.TotalStars += newCourse.Rating;
                    course.Rating = Convert.ToDecimal(course.TotalStars) / Convert.ToDecimal(course.TotalReviews);

                    db?.StudentCourses?.Update(oldStudentCourse);
                    db.Course.Update(course);
                }                
            }
            await db.SaveChangesAsync();
            return Redirect("~/home/CourseReviewers/" + newCourse.CourseId);
        }


        [Authorize(Roles = "Student")]
        [HttpGet]
        [ActionName("DeleteReview")]
        public async Task<IActionResult> DeleteReview(int id)//StudentCourseId
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);
            var oldStudentCourse = db.StudentCourses.Where(sc=>sc.Id==id&&sc.StudentId==userId)?.FirstOrDefault();
            if (oldStudentCourse != null)
            {
                var course = db?.Course.Find(oldStudentCourse.CourseId);
                if (course != null)
                {
                    oldStudentCourse.CourseId = oldStudentCourse.CourseId;
                    oldStudentCourse.StudentId = userId;                    
                    oldStudentCourse.ReviewText = null;
                    oldStudentCourse.ReviewDate = null;
                    oldStudentCourse.IsReviewed = false;

                    course.TotalReviews -= 1;
                    course.TotalStars -= oldStudentCourse.Rating;
                    if (course.TotalReviews >= 1)
                    {
                        course.Rating = Convert.ToDecimal(course.TotalStars) / Convert.ToDecimal(course.TotalReviews);
                    }
                    else
                    {
                        course.TotalStars = 0;
                        course.Rating = 0;
                    }

                    oldStudentCourse.Rating = 0;
                    db?.StudentCourses?.Update(oldStudentCourse);
                    db?.Course.Update(course);
                    await db?.SaveChangesAsync();
                    return Redirect("~/home/CourseReviewers/" + oldStudentCourse.CourseId);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            ViewBag.SelectedPage = "homeNavItem";
            await signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToAction(nameof(Index));
            }
        }
    }
}