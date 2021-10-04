using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap
{
    public enum ElementsToSearch
    {
        Nodes,
        Ways,
        Relations,
        NodesAndWays,
        NodesAndRelations,
        WaysAndRelations,
        All
    }
}
