using BH.oM.Base;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap
{
    public class Response : BHoMObject
    {
        public virtual FeatureCollection FeatureCollection { get; set; } = new FeatureCollection();

        public virtual string Report { get; set; } = "";
    }
}
