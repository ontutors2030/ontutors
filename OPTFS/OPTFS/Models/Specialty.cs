using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class Specialty
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public virtual List<Subject>? Subjects { get; set; }

        public virtual List<ApplicationUser>? Users { get; set; }

        public bool Active { get; set; }
    }
}
