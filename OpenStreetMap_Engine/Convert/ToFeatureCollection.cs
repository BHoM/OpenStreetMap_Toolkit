using BH.oM.Geospatial;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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
                    m_Points = new List<Feature>();
                    foreach (CustomObject element in elements)
                    {
                        if((string)element.CustomData["type"] == "node")
                        {
                            Feature feature = new Feature();
                            feature.Geometry = ToPoint(element);

                            feature.Properties = GetTags(element);
                            if(!feature.Properties.ContainsKey("OSM_Id"))
                                feature.Properties.Add("OSM_Id", GetOSMId(element));
                            m_Points.Add(feature);
                        }
                            
                    }
                    m_LineString = new List<Feature>();
                    foreach (CustomObject element in elements)
                    {
                        string type = (string)element.CustomData["type"];
                        if (type == "way")
                        {
                            Feature feature = new Feature();
                            feature.Geometry = ToLineString(element);

                            feature.Properties = GetTags(element);
                            if (!feature.Properties.ContainsKey("OSM_Id"))
                                feature.Properties.Add("OSM_Id", GetOSMId(element));
                            m_LineString.Add(feature);
                        }
                    }
                    foreach (CustomObject element in elements)
                    {
                        string type = (string)element.CustomData["type"];
                        if (type == "relation")
                        {
                            Feature feature = new Feature();
                            feature.Geometry = ToMultiLineString(element);

                            feature.Properties = GetTags(element);
                            if (!feature.Properties.ContainsKey("OSM_Id"))
                                feature.Properties.Add("OSM_Id", GetOSMId(element));
                            featureCollection.Features.Add(feature);
                        }
                    }
                    return featureCollection;

                    
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

        private static LineString ToLineString(CustomObject customObject)
        {
            LineString lineString = new LineString();

            List<object> ids = new List<object>();
            if (customObject.CustomData.ContainsKey("nodes"))
                ids = (List<object>)customObject.CustomData["nodes"];

            foreach (object id in ids)
            {

                Feature point = m_Points.Find(x => System.Convert.ToInt64(x.Properties["OSM_Id"]).Equals(System.Convert.ToInt64(id)));
                if (point!=null)
                    lineString.Points.Add((Point)point.Geometry);
            }
            return lineString;
        }

        private static MultiLineString ToMultiLineString(CustomObject customObject)
        {
            MultiLineString lineString = new MultiLineString();
            List<object> members = new List<object>();
            if (customObject.CustomData.ContainsKey("members"))
                members = (List<object>)customObject.CustomData["members"];

            foreach (object member in members)
            {
                CustomObject element = (CustomObject)member;
                long osmid = 0;
                if (element.CustomData["ref"] is int)
                    osmid = (int)element.CustomData["ref"];
                if (element.CustomData["ref"] is long)
                    osmid = (long)element.CustomData["ref"];
                string role = "";
                if (element.CustomData.ContainsKey("role"))
                    role = (string)element.CustomData["role"];
                
                
                Feature ls = m_LineString.Find(x => System.Convert.ToInt64(x.Properties["OSM_Id"]).Equals(System.Convert.ToInt64(osmid)));
                if (ls != null)
                    lineString.LineStrings.Add((LineString)ls.Geometry);
            }
            return lineString;
        }

        private static long GetOSMId(CustomObject customObject)
        {
            long id = 0;
            if (customObject.CustomData.ContainsKey("id"))
            {
                if (customObject.CustomData["id"] is int)
                    id = (int)customObject.CustomData["id"];
                if (customObject.CustomData["id"] is long)
                    id = (long)customObject.CustomData["id"];
            }
            
            return id;
        }

        /***************************************************/
        private static Dictionary<string,object> GetTags(CustomObject customObject)
        {

            if (customObject.CustomData.ContainsKey("tags"))
            {
                CustomObject tags = (CustomObject)customObject.CustomData["tags"];
                return tags.CustomData;
            }
            return new Dictionary<string, object>();
        }

        private static List<Feature> m_Points = new List<Feature>();
        private static List<Feature> m_LineString = new List<Feature>();
    }
}