using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.GeoJSON
{
    [Description("Wrapper class for the GeoJSON Polygon. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class Polygon : IGeoJSON
    {
        public virtual string Type { get; set; } = "Polygon";

        public virtual List<List<List<double>>> Coordinates { get; set; } = new List<List<List<double>>>();
    }
}
