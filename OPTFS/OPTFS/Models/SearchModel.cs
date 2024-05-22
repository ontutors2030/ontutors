using System.ComponentModel.DataAnnotations;

namespace OPTFS.Models
{
    public class SearchModel
    {
        [Display(Name = "SearchText")]
        public string? SearchText { get; set; }

        [Display(Name = "Specialty")]
        public int? SpecialtyId { get; set; }
        
        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }
    }
}
