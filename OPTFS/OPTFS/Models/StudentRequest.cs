namespace OPTFS.Models
{
    public class StudentRequest
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? StudentId { get; set; }
        public virtual ApplicationUser? Student { get; set; }
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }        
        public int? StatusId { get; set; }
    }
}