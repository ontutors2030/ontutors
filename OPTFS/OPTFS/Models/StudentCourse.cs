namespace OPTFS.Models
{
    public class StudentCourse
    {
        public int Id { get; set; }
        public int Rating { get; set; } = default;
        public string? ReviewText { get; set; }
        public bool IsReviewed { get; set; } = default;
        public DateTime? ReviewDate { get; set; }
        public string? StudentId { get; set; }
        public virtual ApplicationUser? Student { get; set; }
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public int? RequestId { get; set; }
        public virtual StudentRequest? Request { get; set; }        
    }
}
