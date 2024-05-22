namespace OPTFS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? Content {  get; set; }
        public string? Link {  get; set; }
        public bool Viewed { get; set; }
        public bool Opened { get; set; }
        public int TypeId {  get; set; }                
        public string? ReferantialId { get; set; }
        public DateTime? CreatedDate { get; set; }       
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public string? TutorId { get; set; }

        public Notification()
        {
            
        }
    }
}
