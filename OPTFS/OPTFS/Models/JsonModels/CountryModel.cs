namespace OPTFS.Models.JsonModels
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CityModel> Cities { get; set; }
    }
}
