using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(255)]
        [Display(Name= "Logo")]
        public string? LogoUrl { get; set; }

        public int? SpecialtyId { get; set; }
        public virtual Specialty? Specialty { get; set; }

        public bool Active { get; set; }
    }
}
