using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext db;

        public NotificationsController(ApplicationDbContext context)
        {
            this.db = context;
        }

        [Authorize]
        // GET: Notifications
        public IActionResult Index()
        {
            ViewBag.SelectedPage = "notificationNavItem";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = db.Notification.Include(n => n.User).Where(n=>n.UserId == userId)?.ToList();
            return View(notifications);
        }

        [Authorize]
        public async Task<IActionResult> NotificationsPartial(bool New = false)
        {
            List<Notification> notifications = new List<Notification>();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (New)
            {
                notifications = db.Notification.Include(n => n.User)
                .Where(n => n.UserId == userId).Where(n => !n.Opened && !n.Viewed).OrderByDescending(n => n.Id).ToList();                
            }
            else
            {
                notifications = db.Notification.Include(n => n.User).Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Id).ToList();                
            }
            //UpadateNotifications(notifications,false);

            var myNotifications = notifications.Where(n => !n.Viewed || (!n.Opened)).ToList();
            foreach (var notification in myNotifications)
            {
                notification.Viewed = true;
                /*if (Open)
                    notification.Opened = true;*/
                db.Notification.Update(notification);
            }

            //db.Notification.UpdateRange(myNotifications);
            await db.SaveChangesAsync();
            return PartialView(notifications);
        }

        private async void UpadateNotifications(List<Notification> notificationsList,bool Open)
        {
            var myNotifications= notificationsList.Where(n=>!n.Viewed||(!n.Opened && Open)).ToList();
            foreach (var notification in myNotifications) 
            {                
                notification.Viewed = true;
                if(Open)
                    notification.Opened= true;
                db.Notification.Update(notification);
            }

            //db.Notification.UpdateRange(myNotifications);
            await db.SaveChangesAsync();
        }

        // GET: Notifications/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.SelectedPage = "notificationNavItem";
            if (id == null)
            {
                return NotFound();
            }

            var notification = await db.Notification.Include(n => n.User).Where(n=>n.UserId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            notification.Viewed = true;
            notification.Opened = true;
            return View(notification);
        }                       
    }
}