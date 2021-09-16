using BH.oM.Base;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Geospatial
{
    public static partial class Convert
    {
        [Description("Convert a CustomObject based on a GeoJSON formatted string to a BHoM Geospatial object.")]
        public static IGeospatial ToGeospatial(CustomObject customObject)
        {
            string gType = (string)customObject.CustomData["type"];
            switch (gType)
            {
                case "Point":
                    return ToPoint(customObject);
                case "MultiPoint":
                    return ToMultiPoint(customObject);
                case "Polygon":
                    return ToPolygon(customObject);
                case "MultiPolygon":
                    return ToMultiPolygon(customObject);
                case "LineString":
                    return ToLineString(customObject);
                case "MultiLineString":
                    return ToMultiLineString(customObject);
                case "GeometryCollection":
                    return  ToGeometryCollection(customObject);
                case "FeatureCollection":
                    return ToFeatureCollection(customObject);
                case "Feature":
                    return ToFeature(customObject);
            }
            Reflection.Compute.RecordError("The CustomObject could not be converted to a GeoSpatial Object");
            return null;
        }
    }
}
