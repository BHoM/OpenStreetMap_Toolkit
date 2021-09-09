using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.Nominatim
{
    [Description("Specify the output format of geometry returned by a nominatim API call see here: https://nominatim.org/release-docs/develop/api/Search/#polygon-output, for details.")]
    public enum PolygonOutput
    {
        GeoJSON,
        Kml,
        None,
        Svg,
        Text
    }
}
