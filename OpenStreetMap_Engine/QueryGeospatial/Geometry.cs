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

        [Description("Convert any geospatial to geometry in the plane coordinates determined by the conversion of Longitude to X and Latitude to Y.")]
        public static IGeometry Geometry(this IGeospatial geospatial)
        {
            return geospatial.IToUTM(false);
        }

        /***************************************************/

    }
}
