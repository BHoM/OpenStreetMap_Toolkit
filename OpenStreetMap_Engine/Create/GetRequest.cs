using BH.oM.Adapters.HTTP;
using BH.oM.Adapters.OpenStreetMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        public static GetRequest GetRequest(OverpassRequest request, OpenStreetMapConfig config)
        {

            StringBuilder q = new StringBuilder();
            q.Append(BaseUri());
            q.Append("(");
            q.Append(GetElements(Convert.ToQLString(request.Category, request.Subtype), Convert.ToQLString(request.GeospatialRegion), config));
            q.Append(");");
            q.Append("(._;");
            q.Append(">;");
            q.Append(");");
            q.Append("out body;");
            return new GetRequest(){BaseUrl = q.ToString(), Parameters = null };
        }

        /***************************************************/

        private static string GetElements(string tagfilter, string regionQuery, OpenStreetMapConfig config)
        { 
            switch(config.ElementsToSearch)
            {
                case ElementsToSearch.All:
                    return "rel" + tagfilter + regionQuery + "node" + tagfilter + regionQuery + "way" + tagfilter + regionQuery;
                case ElementsToSearch.Nodes:
                    return "node" + tagfilter + regionQuery;
                case ElementsToSearch.Ways:
                    return "way" + tagfilter + regionQuery;
                case ElementsToSearch.Relations:
                    return "rel" + tagfilter + regionQuery;
                case ElementsToSearch.NodesAndWays:
                    return "node" + tagfilter + regionQuery + "way" + tagfilter + regionQuery;
                case ElementsToSearch.NodesAndRelations:
                    return "node" + tagfilter + regionQuery + "rel" + tagfilter + regionQuery;
                case ElementsToSearch.WaysAndRelations:
                    return "way" + tagfilter + regionQuery + "rel" + tagfilter + regionQuery;
                default:
                    return "";
            }
        }
    }
}
