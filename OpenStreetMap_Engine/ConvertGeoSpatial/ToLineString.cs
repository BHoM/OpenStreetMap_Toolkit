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

        [Description("Convert geoJSON formatted coordinate object to BHoM Geospatial LineString.")]
        public static IGeospatial ToLineString(object coordinates)
        {
            LineString lineString = new LineString();

            lineString.Points = GetCoordSet(coordinates);

            return lineString;
        }

        /***************************************************/
    }
}
