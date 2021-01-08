using CsvHelper.Configuration.Attributes;

namespace UsingTheGeoJSONFormatInDotNet_Demo
{
    // because the property names and column names don't match, we want to use the json property attribute
    // we're also ignoring some things in the file we don't care about, like phone number
    internal class StarbucksLocationsWorldwideFileRow
    {
        [Name("Brand")]
        public string Brand { get; set; }

        [Name("Store Number")]
        public string StoreNumber { get; set; }

        [Name("Store Name")]
        public string StoreName { get; set; }

        [Name("Ownership Type")]
        public string OwnershipType { get; set; }

        [Name("City")]
        public string City { get; set; }

        [Name("State/Province")]
        public string StateOrProvince { get; set; }

        [Name("Country")]
        public string Country { get; set; }

        [Name("Longitude")]
        public double? Longitude { get; set; }

        [Name("Latitude")]
        public double? Latitude { get; set; }

    }
}