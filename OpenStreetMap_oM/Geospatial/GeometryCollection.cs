using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON Geometry Collection. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class GeometryCollection : IGeospatial
    {
        [Description("The collection of Geospatial geometries.")]
        public virtual List<IGeospatial> Geometries { get; set; } = new List<IGeospatial>();
    }
}
