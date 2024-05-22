using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;
using System.Security.Claims;

namespace OPTFS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public AdminController(UserManager<ApplicationUser> _userManager,
            ApplicationDbContext context)
        {
            this.db = context;
            this.userManager = _userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult TutorRequests()
        {
            ViewBag.SelectedPage = "tutorRequestsNavItem";
            var requests = userManager.Users.
                Include(u => u.Country).
                Include(u => u.City).
                Include(u => u.Specialty).
                Include(u => u.UserFiles).Where(u => u.UserTypeId == "Tutor" && u.IsNeedToReview == true)
                .OrderByDescending(u => u.ReviewRequestDateTime).ToList();

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Notification> notificationList = new List<Notification>();
            foreach (var request in requests)
            {
                var notifications = db.Notification.Where(n => n.TypeId ==(int)NotificationType.TutorNewRequest
                /*&& n.UserId == userId*/ && n.TutorId == request.Id).ToList();
                if(notifications!=null && notifications.Any())                
                    notificationList.AddRange(notifications);                
            }

            if (notificationList.Count > 0)
                this.UpadateNotifications(notificationList, false);
            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptRequests(string id)
        {
            ApplicationUser? applicationUser = userManager.Users.Where(u => u.Id == id)?.FirstOrDefault();
            if (applicationUser != null)
            {
                applicationUser.IsNeedToReview = false;
                applicationUser.IsTutorConfirmed = true;
                await userManager.UpdateAsync(applicationUser);

                Notification notification = new Notification
                {
                    Content = "Your request accepted",
                    TypeId = (int)NotificationType.TutorAccepted,
                    CreatedDate = DateTime.Now,
                    Opened = false,
                    Viewed = false,                    
                    UserId = id
                };
                db.Notification.Add(notification);
                await db.SaveChangesAsync();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = db.Notification.Where(n => n.TypeId == (int)NotificationType.TutorNewRequest && n.UserId==userId && n.TutorId == id).ToList();
            this.UpadateNotifications(notifications, true);
            return RedirectToAction(nameof(TutorRequests));
        }

        private async void UpadateNotifications(List<Notification> notificationsList, bool Open)
        {
            var notifications = notificationsList.Where(n => !n.Viewed || (!n.Opened && Open)).ToList();
            foreach (var notification in notifications)
            {
                notification.Viewed = true;
                if (Open)
                    notification.Opened = true;
                db.Notification.Update(notification);
            }

            //db.Notification.UpdateRange(notifications);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectRequests(string id)
        {
            ApplicationUser? applicationUser = userManager.Users.Where(u => u.Id == id)?.FirstOrDefault();
            if (applicationUser != null)
            {
                applicationUser.IsNeedToReview = false;
                applicationUser.IsTutorConfirmed = false;
                await userManager.UpdateAsync(applicationUser);

                Notification notification = new Notification
                {
                    Content = "Your request rejected",
                    TypeId = (int)NotificationType.TutorRejected,
                    CreatedDate = DateTime.Now,
                    Opened = false,
                    Viewed = false,
                    UserId = id
                };
                db.Notification.Add(notification);
                await db.SaveChangesAsync();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = db.Notification.Where(n => n.TypeId == (int)NotificationType.TutorNewRequest && n.UserId == userId && n.TutorId == id).ToList();
            this.UpadateNotifications(notifications, true);
            return RedirectToAction(nameof(TutorRequests));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageCourses()
        {
            ViewBag.SelectedPage = "courseNavItem";
            var courses = db.Course.Include(c => c.Subject).Include(c => c.Tutor);
            return View(await courses.ToListAsync());            
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int? id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var course = db.Course
                .Include(c => c.StudentCourses)
                .Include(c => c.CourseDetails)
                .Include(c => c.StudentRequests)
                .Where(c => c.Id == id)?.FirstOrDefault();
            
            return View(course);
        }

        [Authorize(Roles = "Admin")]        
        [HttpPost, ActionName("DeleteCourse")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.SelectedPage = "courseNavItem";
            var course = db.Course
                .Include(c => c.StudentCourses)
                .Include(c => c.CourseDetails)
                .Include(c => c.StudentRequests)
                .Where(c => c.Id == id)?.FirstOrDefault();

            course.CourseDetails = db.CourseDetail.Where(cd => cd.CourseId == id)?.ToList();
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

            if (course?.StudentRequests != null)
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
            return RedirectToAction(nameof(ManageCourses));
        }
    }
}
