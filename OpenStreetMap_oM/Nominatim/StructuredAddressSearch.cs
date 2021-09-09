using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.Nominatim
{
    [Description("Alternative query string format split into several parameters for structured requests. " +
        "Not all parameters are required. Structured requests are faster but are less robust against alternative OpenStreetMap tagging schemas.")]
    public class StructuredAddressSearch : BHoMObject
    {
        public virtual string Street { get; set; } = "";
        public virtual string City { get; set; } = "";
        public virtual string County { get; set; } = "";
        public virtual string State { get; set; } = "";
        public virtual string Country { get; set; } = "";
        public virtual string PostalCode { get; set; } = "";
    }
}
