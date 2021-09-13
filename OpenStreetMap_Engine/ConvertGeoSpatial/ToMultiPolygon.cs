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

        [Description("Convert geoJSON formatted coordinate object to BHoM Geospatial MultiPolygon.")]

        public static IGeospatial ToMultiPolygon(object coordinates)
        {
            MultiPolygon multipolygon = new MultiPolygon();
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;

            foreach (object c in coords)
                multipolygon.Polygons.Add((Polygon)ToPolygon(c));

            return multipolygon;
        }

        /***************************************************/
    }
}
