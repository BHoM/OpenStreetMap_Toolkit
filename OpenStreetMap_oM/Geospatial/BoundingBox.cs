using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing a geospatial bounding box in The World Geodetic System (WGS 1984, EPSG:4326).")]
    public class BoundingBox : IGeospatial
    {
        [Description("The lower bound values for the Longitude, Latitude and Altitude coordinates of the Box corner Points.")]
        public virtual Point Min { get; set; } = new Point();

        [Description("The upper bound values for the Longitude, Latitude and Altitude coordinates of the Box corner Points.")]
        public virtual Point Max { get; set; } = new Point();
    }
}
