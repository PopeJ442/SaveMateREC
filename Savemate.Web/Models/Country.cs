namespace Savemate.Web.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string TwoLetterCode { get; set; } // e.g., "GH", "US"
        public string ThreeLetterCode { get; set; } // e.g., "GHA", "USA"
        public string NumericCode { get; set; }
        public string Region { get; set; }
        public string CurrencyCode { get; set; }
        public string CallingCode { get; set; }
    }
}
