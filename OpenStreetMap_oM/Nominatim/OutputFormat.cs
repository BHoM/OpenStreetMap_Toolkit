using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.Nominatim
{
    [Description("Specify the output format of a nominatim API call see here: https://nominatim.org/release-docs/develop/api/Output/, for details.")]
    public enum OutputFormat
    {
        [Description("Format follows the RFC7946. Every feature includes a bounding box (bbox).")]
        GeoJSON,
        [Description("Format follows the GeocodeJSON specification 0.1.0.")]
        GeoCodeJson,
        [Description("Returns an array of places (for location search from a textual description or address) or a single place (when searching for location details by latitude and longitude).")]
        Json,
        [Description("Same format as the JSON format with two changes: class renamed to category and additional field place_rank with the search rank of the object.")]
        Jsonv2,
        [Description("Format returns one or more place objects in slightly different formats depending on the API call.")]
        Xml
    }
}
