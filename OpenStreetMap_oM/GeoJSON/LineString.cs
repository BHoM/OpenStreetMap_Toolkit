using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.GeoJSON
{
    [Description("Wrapper class for the GeoJSON LineString. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class LineString : IGeoJSON
    {
        public virtual string Type { get; set; } = "LineString";

        public virtual List<List<double>> Coordinates { get; set; } = new List<List<double>>();
    }
}
