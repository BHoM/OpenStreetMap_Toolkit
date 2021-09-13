using BH.oM.Geometry;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Geospatial
{
    public static partial class Query
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Convert any geospatial to geometry in the Universal Transverse Mercator projection. Where a single element spans two grid zones results maybe unexpected.")]
        public static IGeometry Geometry(this IGeospatial geospatial)
        {
            return geospatial.IToUTM();
        }

        /***************************************************/

    }
}
