using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing the GeoJSON feature. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class Feature : BHoMObject
    {
        [Description("Properties associated with this Geospatial feature.")]
        public virtual Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        [Description("The BoundingBox containing all Geospatial geometry of the feature.")]
        public virtual BoundingBox BoundingBox { get; set; } = new BoundingBox();

        [Description("The Geospatial geometry of the feature.")]
        public virtual IGeospatial Geometry { get; set; }

    }
}
