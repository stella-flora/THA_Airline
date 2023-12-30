namespace Presentation.Models.ViewModels
{
    public class FlightViewModel
    {
        public int Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double WholesalePrice { get; set; }
        public double ComissionRate { get; set; }
        public double RetailPrice { get; set; }
    }
}
