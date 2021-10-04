using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap
{
    [Description("Define configuration settings for pulling OpenStreetMap data using the OpenStreetMap Adapter")]
    public class OpenStreetMapConfig : ActionConfig
    {
        [Description("Elements are the basic components of OpenStreetMap's conceptual data model of the physical world. This will limit the search to some combination of those elements." +
            "The default is to search for all")]
        public virtual ElementsToSearch ElementsToSearch { get; set; } = ElementsToSearch.All;
    }
}
