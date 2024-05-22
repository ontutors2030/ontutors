using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;
using System.Security.Claims;

namespace OPTFS.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public ProfileController(UserManager<ApplicationUser> _userManager,
            ApplicationDbContext context)
        {
            this.db = context;
            this.userManager = _userManager;
        }

        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> TutorProfile()
        {
            ViewBag.SelectedPage = "qualificationNavItem";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);
            var files = db.UserFile.Where(f => f.UserId == userId && f.FileTypeId == 1 && f.AttachmentTypeId == 2);
            if (files != null)
            {
                applicationUser.UserFiles = files.ToList();
            }
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", applicationUser.SpecialtyId);
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", applicationUser.CountryId);
            ViewData["CityId"] = new SelectList(db.City, "Id", "Name", applicationUser.CityId);
            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> updateTutorProfile(//List<IFormFile> uploadFiles, 
            [Bind("Experience,Qualifications,FullName")] ApplicationUser UserInfo)
        {
            //var x = uploadFiles;
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);
            if (applicationUser != null)
            {
                applicationUser.FullName = UserInfo.FullName;
                applicationUser.Experience = UserInfo.Experience;
                applicationUser.Qualifications = UserInfo.Qualifications;
                applicationUser.IsNeedToReview = true;
                applicationUser.IsTutorConfirmed = false;
                applicationUser.ReviewRequestDateTime = DateTime.Now;
                await userManager.UpdateAsync(applicationUser);

                //var admin=userManager.Users.Where(u=>u.Email == SharedStatics.AdminEmail).FirstOrDefault();
                var admin = userManager.Users.Where(u => u.UserTypeId=="Admin").FirstOrDefault();
                if (admin != null)
                {
                    Notification notification = new Notification
                    {
                        Content = "Tutor request from '<a href='/Profile/ViewProfile/" + applicationUser.Id + "'>" + applicationUser.FullName + "'</a>",
                        TypeId = (int)NotificationType.TutorNewRequest,
                        CreatedDate = DateTime.Now,
                        Opened = false,
                        Viewed = false,
                        UserId = admin.Id,
                        TutorId=applicationUser.Id
                    };
                    db.Notification.Add(notification);
                }

                List<string> resultUrls = new List<string>();
                if (Request.Form.Files?.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        string url = @"uploads/" + DateTime.Now.Ticks + "_" + file.FileName.GetHashCode() + System.IO.Path.GetExtension(file.FileName);
                        string newPath = @"wwwroot\" + url.Replace('/', '\\');
                        string dir = System.IO.Path.GetDirectoryName(newPath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), newPath);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);

                            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                            UserFile userFile = new UserFile
                            {
                                UserId = applicationUser.Id,
                                FileName = file.FileName,
                                FileTypeId = 1,
                                AttachmentTypeId = 2,
                                Url = url,
                                UploadDate = applicationUser.ReviewRequestDateTime,
                                SizeBytes = stream.Length,
                            };
                            db.UserFile.Add(userFile);
                            resultUrls.Add("../" + url);
                        }
                    }
                }

                await db.SaveChangesAsync();
                return Redirect("~/Home/Index");
            }

            return View(applicationUser);            
        }

        [Authorize(Roles ="Tutor")]
        [HttpPost, ActionName("DeleteFile")]        
        public async Task<int> DeleteFile(int id)
        {            
            var file = await db.UserFile.FindAsync(id);
            string fileUrl = string.Empty;
            if (file != null)
            {
                fileUrl = file.Url;
                db.UserFile.Remove(file);
            }
            await db.SaveChangesAsync();
            if(!string.IsNullOrWhiteSpace(fileUrl))
            {
                fileUrl = @"wwwroot\" + fileUrl.Replace('/', '\\');
                var path = Path.Combine(Directory.GetCurrentDirectory(), fileUrl);                
                if(System.IO.File.Exists(path))
                {
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch(Exception Ex)
                    {

                    }
                }
                else
                {

                }
            }
            return 100;
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ViewTutorProfile(string?id)
        {
            ViewBag.SelectedPage = "searchInTutorNavItem";
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? applicationUser = userManager.Users.
                Include(u => u.Specialty).
                Include(u => u.Courses).
                Include(u => u.Country).
                Include(u => u.City).
                Where(u=>u.Id== id).FirstOrDefault();
            var files = db.UserFile.Where(f => f.UserId == id && f.FileTypeId == 1 && f.AttachmentTypeId == 2);
            if (files != null)
            {
                applicationUser.UserFiles = files.ToList();
            }
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", applicationUser.SpecialtyId);
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", applicationUser.CountryId);
            ViewData["CityId"] = new SelectList(db.City, "Id", "Name", applicationUser.CityId);
            return View(applicationUser);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> TutorCourseDetails(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.SelectedPage = "searchInTutorNavItem";
            var courese = db.Course.Find(id);
            ViewBag.TutorId = courese.TutorId;
            var courseDetail = db.CourseDetail.Where(c => id == null || c.CourseId == id).Include(c => c.Course);
            return View(await courseDetail.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> ViewProfile(string? id)
        {
            ViewBag.SelectedPage = "searchInTutorNavItem";
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? applicationUser = userManager.Users.
                Include(u => u.Specialty).
                Include(u => u.Courses).
                Include(u => u.Country).
                Include(u => u.City).
                Where(u => u.Id == id).FirstOrDefault();
            var files = db.UserFile.Where(f => f.UserId == id && f.FileTypeId == 1 && f.AttachmentTypeId == 2);
            if (files != null)
            {
                applicationUser.UserFiles = files.ToList();
            }           
            return View(applicationUser);
        }
    }
}
