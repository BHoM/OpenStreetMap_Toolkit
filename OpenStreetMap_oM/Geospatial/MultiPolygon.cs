using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON MultiPolygon. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class MultiPolygon : IGeospatial
    {
        [Description("Collection of Polygons.")]
        public virtual List<Polygon> Polygons { get; set; } = new List<Polygon>();
    }
}
