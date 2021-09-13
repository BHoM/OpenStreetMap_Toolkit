using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON LineString. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class LineString : IGeospatial
    {
        [Description("Two or more points defining the coordinates of the line string.")]
        public virtual List<Point> Points { get; set; } = new List<Point>();
    }
}
