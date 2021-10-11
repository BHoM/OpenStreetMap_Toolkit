using BH.oM.Base;
using BH.oM.Geospatial;
using BH.oM.Data.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BH.oM.Adapters.OpenStreetMap
{
    public class OverpassRequest : BHoMObject, IRequest
    {
        [Description("OpenStreetMap Category to search for. Categories are equivalent to OpenStreetMap 'keys'. See https://wiki.openstreetmap.org/wiki/Map_features for full documentation.")]
        public virtual string Category { get; set; } = "";

        [Description("Optional OpenStreetMap type to search for. Types are equivalent to OpenStreetMap 'values'. See https://wiki.openstreetmap.org/wiki/Map_features for full documentation." +
            "Default value is \"\" and the search will look for all Subtypes for the provided category.")]
        public virtual string Type { get; set; } = "";

        [Description("Geospatial region to search.")]
        public virtual IGeospatialRegion GeospatialRegion { get; set; } = null;
    }
}
