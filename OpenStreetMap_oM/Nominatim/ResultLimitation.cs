using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.Nominatim
{
    public class ResultLimitation : BHoMObject
    {
        [Description("Limit search results to one or more countries.must be the ISO 3166-1alpha2 code, e.g. gb for the United Kingdom, de for Germany.")]
        public virtual List<string> CountryCodes { get; set; } = new List<string>();

        [Description("Limit the number of returned results. Default: 10, Maximum: 50.")]
        public virtual int Limit { get; set; } = 10;

        [Description("The preferred area to find search results.")]
        public virtual BoundingBox BoundingBox { get; set; } = null;
    }
}
