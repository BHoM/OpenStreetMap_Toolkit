using BH.oM.Geospatial;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BH.Engine.Geospatial
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Convert a CustomObject based on a geoJSON formatted string to BHoM Geospatial Feature.")]
        public static Feature ToFeature(CustomObject customObject)
        {
            object fType;
            customObject.CustomData.TryGetValue("type", out fType);

            if (fType == null || ((string)fType).ToLower() != "feature")
            {
                Reflection.Compute.RecordError("Object was not a feature type.");
                return null;
            }

            Feature feature = new Feature();
            if (customObject.CustomData.ContainsKey("properties"))
            {
                object props;
                customObject.CustomData.TryGetValue("properties", out props);
                feature.Properties = ((CustomObject)props).CustomData;
            }
            if (customObject.CustomData.ContainsKey("geometry"))
            {
                CustomObject geometry = (CustomObject)customObject.CustomData["geometry"];
                string gType = (string)geometry.CustomData["type"];
                if (gType == "GeometryCollection")
                    feature.Geometry = ToGeometryCollection(geometry);
                else
                {
                    object coordinates = geometry.CustomData["coordinates"];
                    feature.Geometry = ToGeospatial(gType, coordinates);
                }
                
                
            }
            if (customObject.CustomData.ContainsKey("bbox"))
            {
                //box should be list of coordinates per https://datatracker.ietf.org/doc/html/rfc7946#section-5
                List<object> bbox = (List<object>)customObject.CustomData["bbox"];
                feature.BoundingBox = (BoundingBox)ToBoundingBox(bbox);
            }
            return feature;

        }

        /***************************************************/
        private static IGeospatial ToGeospatial(string type, object geoJSONCoordinates)
        {
            switch (type)
            {
                case "Point":
                    return ToPoint(geoJSONCoordinates);
                case "MultiPoint":
                    return ToMultiPoint(geoJSONCoordinates);
                case "Polygon":
                    return ToPolygon(geoJSONCoordinates);
                case "MultiPolyon":
                    return ToMultiPolygon(geoJSONCoordinates);
                case "LineString":
                    return ToLineString(geoJSONCoordinates);
                case "MultiLineString":
                    return ToMultiLineString(geoJSONCoordinates);
            }
            return null;
        }
    }
}
