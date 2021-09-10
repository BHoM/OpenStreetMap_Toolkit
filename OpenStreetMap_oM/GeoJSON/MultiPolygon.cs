using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.GeoJSON
{
    [Description("Wrapper class for the GeoJSON MultiPolygon. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class MultiPolygon : IGeoJSON
    {
        public virtual string Type { get; set; } = "MultiPolygon";

        public virtual List<List<List<List<double>>>> Coordinates { get; set; } = new List<List<List<List<double>>>>();
    }
}
