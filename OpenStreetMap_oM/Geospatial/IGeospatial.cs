using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("The parent interface for all primitive Geospatial objects." +
                 "\nIGeospatial implements IObject - and not IBHoMObject. Equally primitive Geospatial objects do not inherit from the base BHoMObject class either. As primitives, the additional base BHoM properties are omitted for both efficiency and performance.")]
    public interface IGeospatial : IObject
    {
    }
}
