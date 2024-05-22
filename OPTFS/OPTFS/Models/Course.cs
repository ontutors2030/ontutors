using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class Course
    {
        private readonly ApplicationDbContext? db;
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Name { get; set; }
        
        [Required]        
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a number")]
        public decimal? Price { get; set; }

        [Required]        
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a number")]
        public decimal? Discount { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(255)]
        [Display(Name ="Logo")]
        public string? LogoUrl { get; set; }

        public string? TutorId { get; set; }

        public virtual ApplicationUser? Tutor { get; set; }

        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        public virtual Subject? Subject { get; set; }

        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ToDate { get; set; }

        public bool Sat { get; set; }
        public bool Sun { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set;}
        public bool Wen { get; set; }

        [Display(Name="Thu")]
        public bool Thi { get; set; }
        public bool Fri { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm:ss tt}")]
        [Display(Name = "From Time")]
        public string? FromTime { get; set; }

        [Display(Name = "To Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm:ss tt}")]
        public string? ToTime { get; set; }

        public int TotalStars { get; set; } = default!;
        public decimal Rating {  get; set; } = default!;
        public int TotalReviews { get; set; } = default!;
        public bool Active { get; set; }

        public virtual List<StudentCourse>? StudentCourses { get; set; }  
        public virtual List<StudentRequest>? StudentRequests { get; set; }
        public virtual List<CourseDetail>? CourseDetails { get; set; }
        public Course()
        {

        }

        public Course(ApplicationDbContext context)
        {
            this.db = context;
        }

        public List<StudentCourse>? GetReviews()
        {
            var Result= this.db.StudentCourses.Include(r=>r.Student).Where(r =>r.CourseId==this.Id && r.IsReviewed == true)?.ToList();
            return Result;
        }
    }
}
