using OsmSharp;
using OsmSharp.Streams;
using System.Text.RegularExpressions;

string planetFilePath = @"D:\openstreetmap\planetfile\planet-220919.osm.pbf";

if (args.Length > 0)
{
    planetFilePath = args[0];
}

using (var fileStream = new FileInfo(planetFilePath).OpenRead())
{
    Console.WriteLine($"Starting processing on {planetFilePath}");
    var source = new PBFOsmStreamSource(fileStream);

    
    int progressCount = 0;
    int foundCount = 0;

    foreach (var element in source)
    {
        if (element.Type != OsmSharp.OsmGeoType.Way)
            continue;

        try
        {
            if (element.Tags.ContainsKey("highway") && element.Tags["highway"] == "track")
            {
                if (element.Tags.ContainsKey("bridge") && element.Tags["bridge"] == "yes")
                {
                    if (element.Tags.ContainsKey("layer"))
                    {
                        var layerValue = element.Tags["layer"];

                        if (layerValue == "2" || layerValue =="3" || layerValue == "4")
                        {
                            foundCount++;
                            Console.WriteLine(element.Id);
                        }
                    }
                }
            }

            progressCount++;
            if (progressCount %100000000== 0)
            {
                Console.WriteLine(progressCount);
            }
        }
        catch
        {
            Console.WriteLine($"Error: {element.Id}");
        }
    }

    Console.WriteLine($"Found {foundCount} elements");
}