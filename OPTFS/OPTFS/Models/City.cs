using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Name { get; set; }

        public int? CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public virtual List<ApplicationUser>? Users { get; set; }

        public bool Active { get; set; }
    }
}
