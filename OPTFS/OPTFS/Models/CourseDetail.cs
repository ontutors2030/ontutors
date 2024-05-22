using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class CourseDetail
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Name { get; set; }

        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }

        public int LevelIndex { get; set; }        
    }
}
