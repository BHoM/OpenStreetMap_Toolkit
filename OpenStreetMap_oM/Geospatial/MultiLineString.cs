using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON MultiLineString. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class MultiLineString : IGeospatial
    {
        [Description("Collection of LineStrings.")]
        public virtual List<LineString> LineStrings { get; set; } = new List<LineString>();
    }
}
