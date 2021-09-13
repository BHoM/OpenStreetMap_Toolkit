using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON feature collection. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class FeatureCollection : IGeospatial
    {
        [Description("The collection of Features.")]
        public virtual List<Feature> Features { get; set; } = new List<Feature>();
    }
}
