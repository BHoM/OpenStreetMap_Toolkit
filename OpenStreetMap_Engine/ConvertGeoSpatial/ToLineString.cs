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

        [Description("Convert a CustomObject based on a GeoJSON formatted string to BHoM Geospatial LineString.")]
        public static IGeospatial ToLineString(CustomObject customObject)
        {
            if (customObject.CheckObject("LineString"))
            {
                object coordinates = customObject.TopLevelCoordinates();
                if (coordinates != null)
                {
                    LineString lineString = new LineString();
                    lineString.Points = GetCoordSet(coordinates);
                    return lineString;
                }
            }
            return null;
        }

        /***************************************************/

        [Description("Convert GeoJSON formatted coordinate object to BHoM Geospatial LineString.")]
        public static IGeospatial ToLineString(object coordinates)
        {
            LineString lineString = new LineString();

            lineString.Points = GetCoordSet(coordinates);

            return lineString;
        }

        /***************************************************/
    }
}
