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

        [Description("Convert geoJSON formatted coordinate object collection to BHoM Geospatial BoundingBox.")]
        public static IGeospatial ToBoundingBox(List<object> coordinates)
        {
            BoundingBox boundingBox = new BoundingBox();
            List<double> coords = new List<double>();
            foreach (object c in coordinates)
            {
                if (c is double)
                    coords.Add(System.Convert.ToDouble(c));
            }
            if (coords.Count < 4)
            {
                Reflection.Compute.RecordError("Insufficient coordinates to create a bounding box.");
                return null;
            }
            if (coords.Count == 4)
            {
                boundingBox.Min = new Point() { Longitude = coords[0], Latitude = coords[1] };
                boundingBox.Max = new Point() { Longitude = coords[2], Latitude = coords[3] };
            }
            if (coords.Count == 6)
            {
                boundingBox.Min = new Point() { Longitude = coords[0], Latitude = coords[1], Altitude = coords[2] };
                boundingBox.Max = new Point() { Longitude = coords[3], Latitude = coords[4], Altitude = coords[5] };
            }
            return boundingBox;
        }

        /***************************************************/
    }
}
