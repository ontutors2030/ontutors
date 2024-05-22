using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class ApplicationUser : IdentityUser
    {
        private readonly ApplicationDbContext? db;
        #nullable enable
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
        public string? UserTypeId { get; set; }        

        [Display(Name = "Specialty")]
        public int? SpecialtyId { get; set; }

        public virtual Specialty? Specialty { get; set; }

        [MaxLength(255)]
        public string? Qualifications { get; set; }

        [MaxLength(255)]
        public string? Experience { get; set; }

        [MaxLength(255)]
        public string? Style { get; set; }

        public virtual List<Course>? Courses { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        public virtual Country? Country { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        public virtual City? City { get; set; }        
        public bool? IsTutorConfirmed { get; set; }
        public bool? IsNeedToReview { get; set; }
        public DateTime? ReviewRequestDateTime {  get; set; }

        public string? PersonalImageUrl { get; set; }
        public virtual List<UserFile>? UserFiles { get; set; }
        public virtual List<StudentCourse>? StudentCourses { get; set; }

        #region Constructors
        public ApplicationUser()
        {
            
        }        

        public ApplicationUser(ApplicationDbContext context)
        {
            this.db = context;
        }
        #endregion

        #region Notifications Methods
        public List<Notification>? GetNotifications()
        {
            var notifications = db?.Notification.Include(n => n.User).Where(n => n.UserId == this.Id)?.ToList();
            return notifications;
        }

        public int GetNotificationsCount()
        {
            var notifications = db?.Notification.Include(n => n.User).Where(n => n.UserId == this.Id)?.ToList();
            return notifications.Count;
        }

        public List<Notification>? GetNewNotifications()
        {
            var notifications = db?.Notification.Include(n => n.User).Where(n => n.UserId == this.Id)?
                .Where(n => !n.Opened && !n.Viewed).ToList();
            return notifications;
        }

        public int GetNewNotificationsCount()
        {
            var notifications = db?.Notification.Include(n => n.User).Where(n => n.UserId == this.Id)?
                .Where(n=> !n.Opened && !n.Viewed).ToList();
            return notifications.Count;
        }
        #endregion

        #region Student Request Methods
        public bool HasRequest(int CourseId)
        {
            var result= db?.StudentRequest.Where(r=>r.StudentId==this.Id && r.CourseId==CourseId).ToList();
            return result!=null && result.Count>0;
        }

        public bool HasRequest(string StudentId,int CourseId)
        {
            var result = db?.StudentRequest.Where(r => r.StudentId == StudentId && r.CourseId == CourseId).ToList();
            return result != null && result.Count > 0;
        }

        public bool HasRequest(int CourseId, StudentRequestStatus Status)
        {
            var result = db?.StudentRequest.Where(r => r.StudentId == this.Id && r.CourseId == CourseId && r.StatusId == (int)Status).ToList();
            return result != null && result.Count > 0;
        }

        public bool HasRequest(string StudentId, int CourseId, StudentRequestStatus Status)
        {
            var result = db?.StudentRequest.Where(r => r.StudentId == StudentId && r.CourseId == CourseId && r.StatusId == (int)Status).ToList();
            return result != null && result.Count > 0;
        }

        public StudentRequest? GetRequest(int CourseId)
        {
            var result = db?.StudentRequest.Where(r => r.StudentId == this.Id && r.CourseId == CourseId).FirstOrDefault();
            return result;
        }

        public StudentRequest? GetRequest(string StudentId, int CourseId)
        {
            var result = db?.StudentRequest.Where(r => r.StudentId == StudentId && r.CourseId == CourseId).FirstOrDefault();
            return result;
        }

        public StudentRequest? GetRequest(int CourseId, StudentRequestStatus Status)
        {
            var result = db?.StudentRequest.Where(r => r.StudentId == this.Id && r.CourseId == CourseId && r.StatusId == (int)Status).FirstOrDefault();
            return result;
        }

        public StudentRequest? GetRequest(string StudentId, int CourseId, StudentRequestStatus Status)
        {
            var result = db?.StudentRequest.Where(r => r.StudentId == StudentId && r.CourseId == CourseId && r.StatusId == (int)Status).FirstOrDefault();
            return result;
        }
        #endregion

        #region Roles Methods
        public bool HasRele(string RoleId)
        {
            var userRoles=db?.UserRoles.Where(ur=>ur.UserId==this.Id &&  ur.RoleId==RoleId).ToList();
            return userRoles != null && userRoles.Count > 0;
        }
        #endregion

        #region Student Course Methods
        public bool HasCourse(int CourseId)
        {
            var result = db?.StudentCourses.Where(r => r.StudentId == this.Id && r.CourseId == CourseId).ToList();
            return result != null && result.Count > 0;
        }

        public bool HasCourse(string StudentId, int CourseId)
        {
            var result = db?.StudentCourses.Where(r => r.StudentId == StudentId && r.CourseId == CourseId).ToList();
            return result != null && result.Count > 0;
        }

        public bool HasReviewed(int CourseId)
        {
            var result = db?.StudentCourses.Include(r => r.CourseId)
                .Where(r => r.StudentId == this.Id && r.CourseId == CourseId && r.IsReviewed == true)
                .ToList();
            return result != null && result.Count > 0;
        }

        public bool HasReviewed(string StudentId, int CourseId)
        {
            var result = db?.StudentCourses
                .Where(r => r.StudentId == StudentId && r.CourseId == CourseId && r.IsReviewed == true)
                .ToList();
            return result != null && result.Count > 0;
        }
        #endregion

        #region Tutor Rating Methods
        public decimal GetTutorRating()
        {
            decimal totalStars = 0, totalReviews = 0, result = 0;
            var courses = db?.Course.Where(c => c.TutorId == this.Id && c.TotalReviews > 0)?.ToList();
            if (courses != null && courses.Count > 0)
            {
                foreach (var course in courses)
                {
                    totalStars += course.TotalStars;
                    totalReviews+= course.TotalReviews;
                }
                if (totalReviews > 0)
                    result = totalStars / totalReviews;
            }
            return result;
        }

        public decimal GetTutorRating(string TutorId)
        {
            decimal totalStars = 0, totalReviews = 0, result = 0;
            var courses=db?.Course.Where(c=>c.TutorId== TutorId && c.TotalReviews>0)?.ToList();
            if (courses != null && courses.Count>0)
            {
                foreach (var course in courses)
                {
                    totalStars += course.TotalStars;
                    totalReviews += course.TotalReviews;
                }
                if (totalReviews > 0)
                    result = totalStars / totalReviews;
            }
            return result;
        }
        #endregion

        #region Chat Methods
        public List<Chat> GetLastChats()
        {
            var Result=db.Chat
                .Include(c => c.User1).Include(c => c.User2)
                .Where(c=>c.User1Id==this.Id || c.User2Id==this.Id)
                .OrderByDescending(c=>c.UpdatedAt).ToList();
            return Result;
        }

        public int GetNewMessagesCount()
        {
            var messages = db?.ChatMessages.Where(n => n.RecieverId == this.Id)?
                .Where(n => n.StatusId==(int)MessageStatus.New).ToList();
            return messages.Count;
        }
        #endregion
    }
}