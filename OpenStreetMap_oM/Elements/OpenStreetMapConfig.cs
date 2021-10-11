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

        [Description("Global endpoint for OpenStreetMap queries. See https://wiki.openstreetmap.org/wiki/Overpass_API#Public_Overpass_API_instances for more detail. Default is Main ( overpass-api.de)." +
            "If the pull request returns the error \"A task was cancelled\" the endpoint may have crashed and choosing an alternative may provide a result.")]
        public virtual OverpassEndpoint OverpassEndpoint { get; set; } = OverpassEndpoint.Main;
    }
}
