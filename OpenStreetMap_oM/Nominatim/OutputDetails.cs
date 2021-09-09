using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.Nominatim
{
    [Description("Specify the output details included in the results of a nominatim API call see here: https://nominatim.org/release-docs/develop/api/Search/#output-details, for details.")]
    public class OutputDetails : BHoMObject
    {
        [Description("Include a breakdown of the address into elements. Default is false")]
        public virtual bool AddressDetails { get; set; } = true;

        [Description("Include additional information in the result if available, e.g. wikipedia link, opening hours. Default is false")]
        public virtual bool ExtraTags { get; set; } = true;

        [Description("Include a list of alternative names in the results. These may include language variants, references, operator and brand. Default is false")]
        public virtual bool NameDetails { get; set; } = true;
    }
}
