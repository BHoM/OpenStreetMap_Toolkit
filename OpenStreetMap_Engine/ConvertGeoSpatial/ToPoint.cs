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

        [Description("Convert CustomObject from GeoJSON formatted string to BHoM Geospatial Point.")]
        public static IGeospatial ToPoint(CustomObject customObject)
        {
            if (customObject.CheckObject("Point"))
            {
                object coordinates = customObject.TopLevelCoordinates();
                if (coordinates != null)
                    return GetPoint(customObject.CustomData["coordinates"]);
            }
            return null;
        }

        /***************************************************/

        [Description("Convert GeoJSON formatted coordinate object to BHoM Geospatial Point.")]
        public static IGeospatial ToPoint(object geojsonCoordinates)
        {
            return GetPoint(geojsonCoordinates);
        }

        /***************************************************/
    }
}
