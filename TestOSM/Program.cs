using System;
using BH.oM.Osm;
using BH.Engine.Osm;
using BH.oM.HTTP;
using BH.Engine.HTTP;

namespace TestOSM
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryString qs = BH.Engine.Osm.Create.QueryFromPointAndRadiusKeyValue(1000, 53.335087, -6.228350, "amenity", "pub");
            
            string result = BH.Engine.HTTP.Compute.GetRequest(qs.Query);

            OsmObjectContainer osmObjs = BH.Engine.Osm.Create.OsmObjectContainer(result);
        }
    }
}
