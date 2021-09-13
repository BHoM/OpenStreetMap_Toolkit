using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON Polygon. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class Polygon : IGeospatial
    {
        [Description("Collection of closed LineStrings. The first in the collection must be the exterior ring.")]
        public virtual List<LineString> Polygons { get; set; } = new List<LineString>();
    }
}
