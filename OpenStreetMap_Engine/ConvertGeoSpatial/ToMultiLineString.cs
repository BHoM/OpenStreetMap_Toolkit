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

        [Description("Convert geoJSON formatted coordinate object to BHoM Geospatial MultiLineString.")]
        public static IGeospatial ToMultiLineString(object coordinates)
        {
            MultiLineString multiLineString = new MultiLineString();
            //list of coordinates per line string
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;

            foreach (object c in coords)
                multiLineString.LineStrings.Add((LineString)ToLineString(c));

            return multiLineString;
        }

        /***************************************************/
    }
}
