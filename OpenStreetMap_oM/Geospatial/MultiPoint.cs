using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON MultiPoint. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class MultiPoint : IGeospatial
    {
        [Description("Collection of Points.")]
        public virtual List<Point> Points { get; set; } = new List<Point>();
    }
}
