using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap.GeoJSON
{
    [Description("Interface for GeoJSON objects.")]
    public interface IGeoJSON : IObject
    {
        string Type { get; set; }
    }
}
