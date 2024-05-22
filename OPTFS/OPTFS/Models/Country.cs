using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; }
        
        public virtual List<City>? Cities { get; set; }

        public virtual List<ApplicationUser>? Users { get; set; }

        public bool Active { get; set; }
    }
}
