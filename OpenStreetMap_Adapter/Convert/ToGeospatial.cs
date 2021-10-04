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
            Dictionary<long, Feature> ways = new Dictionary<long, Feature>();
            Dictionary<long, Feature> nodes = new Dictionary<long, Feature>();
            
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
                            long id = GetId(element, "id");
                            if ((string)element.CustomData["type"] == "node")
                            {
                                Feature feature = ToFeature(element);
                                nodes.Add(id, feature);
                            }
                            if ((string)element.CustomData["type"] == "way")
                            {
                                Feature feature = ToFeature(element, nodes);
                                ways.Add(id, feature);
                                featureCollection.Features.Add(feature);
                            }
                            if ((string)element.CustomData["type"] == "relation")
                                featureCollection.Features.Add(ToFeature(element, ways, nodes));
                        
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

        public static Feature ToFeature(CustomObject customObject)
        {
            double latitude = (double)customObject.CustomData["lat"];
            double longitude = (double)customObject.CustomData["lon"];
            
            Feature feature = new Feature();
            if (customObject.CustomData.ContainsKey("tags"))
                feature.Properties = ((CustomObject)customObject.CustomData["tags"]).CustomData;
            
            Point node = new Point()
            {
                Latitude = latitude,
                Longitude = longitude,
            };
            feature.Geometry = node;
            return feature;
        }

        public static Feature ToFeature(CustomObject customObject, Dictionary<long, Feature> nodes)
        {
            //convert for an OSM way
            //todo return polygon if closed linestring if open
            Feature feature = new Feature();
            if (customObject.CustomData.ContainsKey("tags"))
                feature.Properties = ((CustomObject)customObject.CustomData["tags"]).CustomData;
            
            List<long> ids = GetIds(customObject, "nodes");

            LineString way = new LineString();
            foreach (long id in ids)
                way.Points.Add((Point)nodes[id].Geometry);
            feature.Geometry = way;
            return feature;
        }

        public static Feature ToFeature(CustomObject customObject, Dictionary<long, Feature> ways, Dictionary<long, Feature> nodes)
        {
            //Convert for an OSM relation
            Feature feature = new Feature();
            if (customObject.CustomData.ContainsKey("members"))
            {
                if(customObject.CustomData.ContainsKey("tags"))
                {
                    CustomObject tags = (CustomObject)customObject.CustomData["tags"];
                    feature.Properties = tags.CustomData;
                }
                string type = "";
                if (feature.Properties.ContainsKey("type"))
                    type = (string)feature.Properties["type"];
                Dictionary<long, string> nodemembers = new Dictionary<long, string>();
                Dictionary<long, string> waymembers = new Dictionary<long, string>();

                foreach (object member in (List<object>)customObject.CustomData["members"])
                {
                    CustomObject element = (CustomObject)member;
                    long osmid = GetId(element, "ref");
                    string role = "";
                    if (element.CustomData.ContainsKey("role"))
                        role = (string)element.CustomData["role"];

                    if ((string)element.CustomData["type"] == "node")
                        nodemembers.Add(osmid, role);
                    if ((string)element.CustomData["type"] == "way")
                        waymembers.Add(osmid, role);
                }
                if(type == "multipolygon")
                {
                    MultiPolygon multiPolygon = new MultiPolygon();
                    foreach (var pair in waymembers)
                        multiPolygon.Polygons.Add((Polygon)ways[pair.Key].Geometry);
                    feature.Geometry = multiPolygon;
                }
                else
                {
                    FeatureCollection featureCollection = new FeatureCollection();
                    foreach (var pair in nodemembers)
                        featureCollection.Features.Add(nodes[pair.Key]);
                    foreach (var pair in waymembers)
                        featureCollection.Features.Add(ways[pair.Key]);
                    feature.Geometry = featureCollection;
                }
                
            }
            return feature;
        }

        private static long GetId(CustomObject custom, string idPropName)
        {
            long osmid = 0;
            if (custom.CustomData[idPropName] is int)
                osmid = (int)custom.CustomData[idPropName];
            if (custom.CustomData[idPropName] is long)
                osmid = (long)custom.CustomData[idPropName];
            return osmid;
        }

        private static List<long> GetIds(CustomObject custom, string property)
        {
            List<long> ids = new List<long>();
            if (custom.CustomData.ContainsKey(property))
            {
                foreach(object obj in (List<object>)custom.CustomData[property])
                {
                    if (obj is long)
                        ids.Add((long)obj);
                    if(obj is int)
                        ids.Add((int)obj);
                }
            }
            return ids;
        }
    }
}
