using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.GeoJSON
{
    [Description("Wrapper class for the GeoJSON Geometry Collection. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class GeometryCollection: IGeoJSON
    {
        public virtual string Type { get; set; } = "GeometryCollection";
        public virtual List<IGeoJSON> Geometries { get; set; } = new List<IGeoJSON>();
    }
}
