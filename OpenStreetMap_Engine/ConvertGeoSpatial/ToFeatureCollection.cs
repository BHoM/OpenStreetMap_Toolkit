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

        [Description("Convert a CustomObject from GeoJSON formatted string to BHoM Geospatial FeatureCollection.")]
        public static FeatureCollection ToFeatureCollection(CustomObject customObject)
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
                    FeatureCollection featureCollection = new FeatureCollection();
                    List<object> features = (List<object>)customObject.CustomData["features"];
                    foreach (object feature in features)
                    {
                        featureCollection.Features.Add(ToFeature(feature as CustomObject));
                    }
                    return featureCollection;
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
        /****           Private Methods                 ****/
        /***************************************************/

        private static List<object> GetList(object coordinates)
        {
            List<object> coords = coordinates as List<object>;
            if (coords == null)
            {
                Reflection.Compute.RecordError("Coordinates could not be converted.");
                return null;
            }
            return coords;
        }

        /***************************************************/

        private static List<Point> GetCoordSet(object coordinates)
        {
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            List <Point> lonlats = new List<Point>();
            foreach (object c in coords)
                lonlats.Add(GetPoint(c));
            return lonlats;
        }

        /***************************************************/

        private static Point GetPoint(object coordinates)
        {
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            if (coords.Count > 2)
                Reflection.Compute.RecordWarning("Point coordinates contains more than 2 values only the first 2 will be used.");
            if (coords.Count < 2)
            {
                Reflection.Compute.RecordError("Insufficient coordinates to create a GeoJSON point.");
                return null;
            }
            double lat = 0;
            double lon = 0;
            try
            {
                lon = System.Convert.ToDouble(coords[0]);
                lat = System.Convert.ToDouble(coords[1]);
            }
            catch
            {
                Reflection.Compute.RecordError("Point coordinates could not be converted to doubles.");
                return null;
            }
            return new Point() { Longitude = lon, Latitude = lat };
        }
    }
}
