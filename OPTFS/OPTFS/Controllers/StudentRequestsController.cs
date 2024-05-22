using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class StudentRequestsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ILogger<HomeController> _logger;
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;

        public StudentRequestsController(ILogger<HomeController> logger,
           RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager, 
           ApplicationDbContext context)
        {
            this._logger = logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.db = context;
        }

        // GET: StudentRequests
        [Authorize(Roles ="Tutor")]
        public async Task<IActionResult> Index()
        {
            ViewBag.SelectedPage = "studentRequestNavItem";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var studentRequests = db.StudentRequest
                .Include(s => s.Student)
                .Include(s=>s.Course)
                .Where(s => s.Course.TutorId == userId);
            return View(await studentRequests.ToListAsync());
        }

        // GET: StudentRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SelectedPage = "studentRequestNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var studentRequest = await db.StudentRequest
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentRequest == null)
            {
                return NotFound();
            }

            return View(studentRequest);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RequestJoin(int? id) // CourseId
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser stuentUser = await userManager.GetUserAsync(User);
            Course course = db.Course.Find(id);
            if (course != null)
            {
                StudentRequest request = new StudentRequest
                {
                    StudentId = userId,
                    CourseId=id,
                    CreatedDate = DateTime.Now,
                    StatusId=(int)StudentRequestStatus.New
                };

                db.StudentRequest.Add(request);
                await db.SaveChangesAsync();

                Notification notification = new Notification
                {
                    Content = "You have new request from '<a href='/Profile/ViewProfile/" + userId+"'>" + stuentUser.FullName+"'</a> to join '"+course.Name+"',",
                    //"<a href='/StudentRequests/Detials/" + request.Id+"' class='btn btn-default'>Details</a>"+
                    //"<a href='/StudentRequests/AcceptRequest/" + request.Id + "' class='btn btn-success'>Accept</a>"+
                    //"<a href='/StudentRequests/RejectRequest/" + request.Id + "' class='btn btn-danger'>Reject</a>",
                    Link= "/Profile/ViewProfile/" + userId,
                    TypeId = (int)NotificationType.StudentRequest,
                    CreatedDate = DateTime.Now,
                    Opened = false,
                    Viewed = false,
                    UserId = course.TutorId
                };

                db.Notification.Add(notification);
                await db.SaveChangesAsync();
            }
            return Redirect("~/Home/Index");
            //return View();
        }

        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> AcceptRequest(int? id)
        {
            StudentRequest request=db.StudentRequest.Include(r=>r.Course).Where(r=>r.Id==id)?.FirstOrDefault();
            if (request != null)
            {
                request.StatusId = (int)StudentRequestStatus.Accepted;
                db.StudentRequest.Update(request);                
                Notification notification = new Notification
                {
                    Content = "You have been accepted in "+
                    "'<a href='/Profile/TutorCourseDetails/" + request.CourseId + "'>"+request.Course.Name+"</a>'",
                    TypeId = (int)NotificationType.StudentAccepted,
                    CreatedDate = DateTime.Now,
                    Opened = false,
                    Viewed = false,
                    UserId = request.StudentId
                };
                db.Notification.Add(notification);
                await db.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> RejectRequest(int? id)
        {
            StudentRequest request = db.StudentRequest.Include(r => r.Course).Where(r => r.Id == id)?.FirstOrDefault();
            if (request != null)
            {
                request.StatusId = (int)StudentRequestStatus.Rejected;
                db.StudentRequest.Update(request);
                Notification notification = new Notification
                {
                    Content = "Sorry,Your request have been reject to join " +
                    "'<a href='/Profile/TutorCourseDetails/" + request.CourseId + "'>" + request.Course.Name + "</a>'",
                    TypeId = (int)NotificationType.StudentRejected,
                    CreatedDate = DateTime.Now,
                    Opened = false,
                    Viewed = false,
                    UserId = request.StudentId
                };
                db.Notification.Add(notification);
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
