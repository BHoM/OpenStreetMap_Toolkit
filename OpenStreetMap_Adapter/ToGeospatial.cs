using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.OpenStreetMap
{
    public static partial class Convert
    {
        public static IGeospatial ToGeospatial(object response)
        {
            Dictionary<long, LineString> ways = new Dictionary<long, LineString>();
            Dictionary<long, Point> nodes = new Dictionary<long, Point>();
            
            //List<Relation> relations = new List<Relation>();
            if (response is CustomObject)
            {
                //custom object with samples
                CustomObject r = response as CustomObject;
                if (r.CustomData.ContainsKey("elements"))
                {
                    FeatureCollection featureCollection = new FeatureCollection();
                    List<object> elements = (List<object>)r.CustomData["elements"];// .Cast<CustomObject>();
                    
                    foreach (object ele in elements)
                    {
                        try
                        {
                            CustomObject element = (CustomObject)ele;
                            CustomObject tags = new CustomObject();
                            long id = 0;
                            if (element.CustomData["id"] is int)
                                id = (int)element.CustomData["id"];
                            if (element.CustomData["id"] is long)
                                id = (long)element.CustomData["id"];
                            if (element.CustomData.ContainsKey("tags"))
                                tags = (CustomObject)element.CustomData["tags"];
                            if ((string)element.CustomData["type"] == "node")
                            {
                                double latitude = (double)element.CustomData["lat"];
                                double longitude = (double)element.CustomData["lon"];
                                Point node = new Point()
                                {
                                    Latitude = latitude,
                                    Longitude = longitude,
                                };
                                //todo store node tags if any
                                //node.AddTags(tags);
                                nodes.Add(id,node);
                            }
                            if ((string)element.CustomData["type"] == "way")
                            {
                                List<object> ids = new List<object>();
                                if (element.CustomData.ContainsKey("nodes"))
                                    ids = (List<object>)element.CustomData["nodes"];
                                LineString way = new LineString()
                                {
                                    Points = nodes.Where(x => ids.Contains(x.Key)).Select(x => x.Value).ToList()
                                };
                            
                                Feature feature = new Feature();
                                feature.Geometry = way;
                                feature.Properties = tags.CustomData;
                                ways.Add(id,way);
                                featureCollection.Features.Add(feature);
                            }
                            if ((string)element.CustomData["type"] == "relation")
                            {
                                List<object> members = new List<object>();
                                if (element.CustomData.ContainsKey("members"))
                                    members = (List<object>)element.CustomData["members"];
                                Feature relation = new Feature();
                                List<LineString> lineStrings = ways.Where(x => members.Contains(x.Key)).Select(x => x.Value).ToList();
                                relation.Geometry = new MultiLineString() { LineStrings = lineStrings};
                                relation.Properties = tags.CustomData;
                                featureCollection.Features.Add(relation);
                            }
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException("Unexpected data format from OpenStreetMap " + e.Message);
                        }
                    }
                    return featureCollection;
                }

            }
            Compute.RecordError("Response was not in the expected format.");
            return null;
        }
    }
}
