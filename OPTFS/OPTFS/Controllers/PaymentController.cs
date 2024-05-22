using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPTFS.Data;
using OPTFS.Models;
using OPTFS.Models.PaymentModels;
using System.Security.Claims;

namespace OPTFS.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly SignInManager<ApplicationUser> signInManager;

        public PaymentController(ILogger<HomeController> logger,
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
        }

        public IActionResult Success()
        {
            return Redirect("~/Home/Index");
        }

        public int Complete()
        {
            return 1;
        }

        [Authorize(Roles ="Student")]        
        [ActionName("SaveCoursePayment")]
        [HttpPost]
        public async Task<bool> SaveCoursePayment(Payment payment)
        {            
            try
            {
                //var uid= userManager.GetUserId(User);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ApplicationUser applicationUser =await userManager.GetUserAsync(User);

                var request = db.StudentRequest.Find(payment.requestId);
                if (request != null)
                {
                    request.StatusId = (int)StudentRequestStatus.Completed;
                    db.StudentRequest.Update(request);
                }

                var course = db.Course.Find(payment.courseId);
                if(request!=null && course != null)
                {
                    StudentCourse studentCourse = new StudentCourse
                    {
                        CourseId = payment.courseId,
                        StudentId = userId,
                        RequestId=payment.requestId
                    };
                    db.StudentCourses?.Add(studentCourse);
                }
                await db.SaveChangesAsync();
                if (string.IsNullOrEmpty(payment.source?.id))
                    payment.source.id = Guid.NewGuid().ToString();

                db.Payments.Add(payment);                
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
