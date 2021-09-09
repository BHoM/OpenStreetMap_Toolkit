using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.Nominatim
{
    [Description("Limit results returned by nominatim API call. See here: https://nominatim.org/release-docs/develop/api/Search/#result-limitation for details.")]
    public class ResultLimitation : BHoMObject
    {
        [Description("Limit search results to one or more countries, must be the ISO 3166-1alpha2 code, e.g. gb for the United Kingdom, de for Germany.")]
        public virtual List<string> CountryCodes { get; set; } = new List<string>();

        [Description("Limit the number of returned results. Default: 10, Maximum: 50.")]
        public virtual int Limit { get; set; } = 50;

        [Description("Exclude OpenStreetMap objects by their Id. Default: No exclusions.")]
        public virtual List<int> ExcludedPlaceIds { get; set; } = new List<int>();

        [Description("The preferred area to find search results.")]
        public virtual BoundingBox BoundingBox { get; set; } = null;

        [Description("Level of detail required for the address. Default: 18. Only applicable to reverse geocoding queries. This is a number that corresponds roughly to the zoom level used in XYZ tile sources in frameworks like Leaflet.js, Openlayers etc.")]
        public virtual int Zoom { get; set; } = 18;
    }
}
