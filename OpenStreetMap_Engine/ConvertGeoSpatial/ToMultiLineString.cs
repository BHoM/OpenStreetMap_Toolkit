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

        [Description("Convert a CustomObject based on a GeoJSON formatted string to BHoM Geospatial MultiLineString.")]
        public static IGeospatial ToMultiLineString(CustomObject customObject)
        {
            if (customObject.CheckObject("MultiLineString"))
            {
                object coordinates = customObject.TopLevelCoordinates();
                if (coordinates != null)
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
            }
            return null;
        }

        /***************************************************/
    }
}
