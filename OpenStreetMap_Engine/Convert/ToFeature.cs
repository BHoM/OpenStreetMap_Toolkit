using BH.oM.Adapters.OpenStreetMap.GeoJSON;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        public static List<Feature> ToFeatures(CustomObject customObject)
        {
            if (customObject == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null object.");
                return null;
            }
            try
            {
                if (customObject.CustomData.ContainsKey("features"))
                {
                    List<Feature> geoJSONFeatures = new List<Feature>();
                    List<object> features = (List<object>)customObject.CustomData["features"];
                    foreach (object feature in features)
                    {
                        geoJSONFeatures.Add(ToFeature(feature as CustomObject));
                    }
                    return geoJSONFeatures;
                }
                else
                    Reflection.Compute.RecordError("No features found in the customObject.");

            }
            catch
            {
                Reflection.Compute.RecordError("Unexpected data format in customObject check OutputFormat is GeoJSON.");
                return null;
            }

            return null;
        }

        /***************************************************/

        public static Feature ToFeature(CustomObject customObject)
        {
            object fType;
            customObject.CustomData.TryGetValue("type", out fType);

            if (fType is null || ((string)fType).ToLower() != "feature")
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
                object coordinates = geometry.CustomData["coordinates"];
                feature.Geometry = IToGeoJSON(gType, coordinates);
                
            }
            if (customObject.CustomData.ContainsKey("bbox"))
            {
                //box should be list of coordinates per https://datatracker.ietf.org/doc/html/rfc7946#section-5
                List<object> bbox = (List<object>)customObject.CustomData["bbox"];
                feature.BoundingBox = (BoundingBox)ToGeoJSONBoundingBox(bbox);
            }
            return feature;

        }
    }
}
