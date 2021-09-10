using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.GeoJSON
{
    [Description("Wrapper class for the GeoJSON feature. See: https://datatracker.ietf.org/doc/html/rfc7946 for more details.")]
    public class Feature : BHoMObject
    {
        public virtual Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public virtual BoundingBox BoundingBox { get; set; } = new BoundingBox();

        public virtual IGeoJSON Geometry { get; set; }

    }
}
