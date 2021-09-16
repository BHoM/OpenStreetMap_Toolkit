using BH.oM.Geospatial;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Concurrent;
using BH.Engine.Base;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Convert CustomObject from OpenStreetMap JSON formatted string to a BHoM Geospatial FeatureCollection.")]
        public static FeatureCollection ToFeatureCollection(CustomObject customObject)
        {
            if (customObject == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null object.");
                return null;
            }
            try
            {
                if (customObject.CustomData.ContainsKey("elements"))
                {
                    FeatureCollection featureCollection = new FeatureCollection();
                    List<object> elements = (List<object>)customObject.CustomData["elements"];
                    m_AddedPoints = new ConcurrentBag<long>();
                    m_AddedLineStrings = new ConcurrentBag<long>();

                    //first convert all the nodes
                    m_Points = new List<Feature>();
                    m_Points.AddRange(ConvertByOSMType("node", elements));

                    //then convert ways that contain references to nodes
                    m_LineString = new List<Feature>();
                    m_LineString.AddRange(ConvertByOSMType("way", elements));

                    //last of all convert relations that contain references to ways
                    featureCollection.Features.AddRange(ConvertByOSMType("relation", elements));

                    //check for ways not associated with relations
                    featureCollection.Features.AddRange(CheckForFeaturesNotAdded(m_LineString, m_AddedLineStrings.ToList()));

                    //check for nodes not associated with ways
                    featureCollection.Features.AddRange(CheckForFeaturesNotAdded(m_Points, m_AddedPoints.ToList()));

                    return featureCollection;
                }
                else
                {
                    Reflection.Compute.RecordError("No OpenStreetMap elements were found.");
                }
            }
            catch
            {
                Reflection.Compute.RecordError("Unexpected data format in customObject.");
                return null;
            }

            return null;
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static List<Feature> CheckForFeaturesNotAdded(List<Feature> converted, List<long> addedIds)
        {
            List<Feature> features = new List<Feature>();
            List<object> convertedIds = converted.Select(s => s.Properties["OSM_Id"]).ToList();
            foreach (object id in convertedIds)
            {
                if (!addedIds.Contains(System.Convert.ToInt64(id)))
                {
                    Feature p = converted.Find(x => System.Convert.ToInt64(x.Properties["OSM_Id"]).Equals(System.Convert.ToInt64(id)));
                    features.Add(p);
                }
            }
            return features;
        }

        /***************************************************/

        private static List<Feature> ConvertByOSMType(string type, List<object> elements)
        {
            ConcurrentBag<Feature> features = new ConcurrentBag<Feature>();
            Parallel.ForEach(elements, x =>
            {
                CustomObject element = x as CustomObject;
                string osmtype = (string)element.CustomData["type"];
                if (osmtype == type)
                {
                    Feature feature = new Feature();
                    switch (type)
                    {
                        case "node":
                            feature.Geometry = ToPoint(element);
                            break;
                        case "way":
                            feature.Geometry = ToLineString(element);
                            break;
                        case "relation":
                            feature.Geometry = ToMultiLineString(element);
                            break;
                    }
                    feature.Properties = GetTags(element);
                    if (!feature.Properties.ContainsKey("OSM_Id"))
                        feature.Properties.Add("OSM_Id", GetOSMId(element));
                    features.Add(feature);
                }
            });
            return features.ToList();
        }

        /***************************************************/

        private static Point ToPoint(CustomObject customObject)
        {
            double latitude = (double)customObject.CustomData["lat"];
            double longitude = (double)customObject.CustomData["lon"];
            Point point = new Point()
            {
                Latitude = latitude,
                Longitude = longitude,
            };
            return point;
        }

        /***************************************************/

        private static LineString ToLineString(CustomObject customObject)
        {
            //todo if the line string is closed we should convert to Polygon 
            LineString lineString = new LineString();

            List<object> ids = new List<object>();
            if (customObject.CustomData.ContainsKey("nodes"))
                ids = (List<object>)customObject.CustomData["nodes"];

            foreach (object id in ids)
            {
                long osmid = System.Convert.ToInt64(id);
                Feature point = m_Points.Find(x => System.Convert.ToInt64(x.Properties["OSM_Id"]).Equals(osmid));
                if (point!=null)
                {
                    if(!m_AddedPoints.Contains(osmid))
                        m_AddedPoints.Add(osmid);
                    lineString.Points.Add((Point)point.Geometry);
                }
                    
            }
            return lineString;
        }

        /***************************************************/
        private static MultiLineString ToMultiLineString(CustomObject customObject)
        {
            //todo if the multi line string contains some open and some closed we should convert to GeometryCollection
            MultiLineString lineString = new MultiLineString();
            List<object> members = new List<object>();
            if (customObject.CustomData.ContainsKey("members"))
                members = (List<object>)customObject.CustomData["members"];

            foreach (object member in members)
            {
                CustomObject element = (CustomObject)member;
                long osmid = 0;
                if (element.CustomData.ContainsKey("ref"))
                    osmid = System.Convert.ToInt64(element.CustomData["ref"]);

                Feature ls = m_LineString.Find(x => System.Convert.ToInt64(x.Properties["OSM_Id"]).Equals(osmid));
                if (ls != null)
                {
                    if (!m_AddedLineStrings.Contains(osmid))
                        m_AddedLineStrings.Add(osmid);
                    lineString.LineStrings.Add((LineString)ls.Geometry);
                }
                    
            }
            return lineString;
        }

        /***************************************************/

        private static object GetOSMId(CustomObject customObject)
        {
            if (customObject.CustomData.ContainsKey("id"))
                return customObject.CustomData["id"];
            return -1;
        }

        /***************************************************/
        private static Dictionary<string,object> GetTags(CustomObject customObject)
        {
            if (customObject.CustomData.ContainsKey("tags"))
            {
                CustomObject tags = (CustomObject)customObject.CustomData["tags"];
                return tags.CustomData.ShallowClone();
            }
            return new Dictionary<string, object>();
        }

        /***************************************************/
        /****           Private Fields                  ****/
        /***************************************************/

        private static List<Feature> m_Points = new List<Feature>();
        private static List<Feature> m_LineString = new List<Feature>();
        private static ConcurrentBag<long> m_AddedPoints = new ConcurrentBag<long>();
        private static ConcurrentBag<long> m_AddedLineStrings = new ConcurrentBag<long>();
    }
}