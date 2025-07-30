using Nager.Country;
using Nager.Country.CountryInfos;
using System.Collections.Generic;

namespace Savemate.Infrastructure
{
    public static class CountryHelper
    {
        public static List<ICountryInfo> GetAllCountries()
        {
            var countryProvider = new CountryProvider();
            var countries = countryProvider.GetCountries();
            return countries.ToList();
        }

    }
}
