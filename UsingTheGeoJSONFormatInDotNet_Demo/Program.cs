using CsvHelper;
using CsvHelper.Configuration;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace UsingTheGeoJSONFormatInDotNet_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // just some basic functionality to show a few features of the GeoJSON.Net library
            Console.WriteLine("Hi. Welcome to a simple GeoJSON demo!");

            // First, I want to play around with some actions using GeoJSON.Net in this console app
            // Install-Package GeoJSON.Net
            // https://github.com/GeoJSON-Net/GeoJSON.Net

       
            // create a position for my local park - this is just representing a point in the park, not a polygon of it
            // TIP: Named properties on hard coded numbers makes it more readable
            Position position = new Position(latitude: 33.749647, longitude: -84.364250);

            // add that poisition to a point
            Point point = new Point(position);

            // convert to json
            string json = JsonConvert.SerializeObject(point);

            Console.WriteLine("GeoJSON Output of my local park:");
            Console.WriteLine(json);

            // convert back to a point
            Point point1 = JsonConvert.DeserializeObject<Point>(json);


            // let's take this python example of converting the "Starbucks Locations Worldwide (2017)" data set to GeoJSON and rewrite it in .NET
            // https://www.kaggle.com/exatasmente/starbucks-geojson
            // https://www.kaggle.com/starbucks/store-locations

            // we have a csv file called StarbucksLocationsWorldwide under ./Resources/StarbucksLocationsWorldwide.csv
            // TIP: when you need a resource file, remember to configure it to copy to the output directory by right clicking it and selecting "Properties"

            var fileLocation = "./Resources/StarbucksLocationsWorldwide.csv";

            if (File.Exists(fileLocation) == false) throw new FileNotFoundException("Cannot Find the StarbucksLocationsWorldwide.csv");

            // we're going to initialize the feature collection here and populate in our using block below
            var starbucksLocations = new FeatureCollection();

            // I'm going to use CSVMapper here, which is one of my favorite libraries - should do a seperate demo on doing csv stuff
            // Install-Package CSVHelper
            // https://joshclose.github.io/CsvHelper/
            using (var reader = new StreamReader(fileLocation))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
            {
                // Fields: Brand,Store Number,Store Name,Ownership Type,Street Address,City,State/Province,Country,Postcode,Phone Number,Timezone,Longitude,Latitude
                var starbucksLocationsWorldwideFileRows = csv.GetRecords<StarbucksLocationsWorldwideFileRow>();

                foreach (var locationFileRow in starbucksLocationsWorldwideFileRows)
                {
                    // create a position from the latitiude and longitude - I made the values in the row mapping nullable because a few locations were being tricky
                    var storePosition = new Position(locationFileRow.Latitude.GetValueOrDefault(), locationFileRow.Longitude.GetValueOrDefault());

                    // create a point to store the position
                    var storePoint = new Point(storePosition);

                    // create a feature to store information about the store along with the point object
                    // we're pasing in the values we care about through an anonymous object
                    // if we wanted to make format the json names, we could use a class with json property attributes to accomodate taht
                    var starbucksLocation = new Feature(storePoint, new {
                        locationFileRow.City,
                        locationFileRow.Country,
                        locationFileRow.OwnershipType,
                        locationFileRow.StateOrProvince,
                        locationFileRow.StoreName,
                        locationFileRow.StoreNumber
                    });


                    // add the location to the locations feature collection
                    starbucksLocations.Features.Add(starbucksLocation);

                }

            }

            // at this point, we've disposed of the stream reader for the original csv file
            // and we have the locations as a GeoJSON string
            var stackbucksLocationsJSON = JsonConvert.SerializeObject(starbucksLocations);

            Console.WriteLine("GeoJSON Output of all the starbucks locations:");
            Console.WriteLine(stackbucksLocationsJSON);

            // and we can convert our GeoJSON string of starbucks locations back to .NET objects
            var deserializedStarbucksLocations = JsonConvert.DeserializeObject<FeatureCollection>(stackbucksLocationsJSON);


            var input = Console.ReadLine();

            
        }
    }
}
