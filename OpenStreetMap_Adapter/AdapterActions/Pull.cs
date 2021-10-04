using BH.Engine.Adapters;
using BH.Engine.Adapters.OpenStreetMap;
using BH.oM.Adapter;
using BH.oM.Adapters.HTTP;
using BH.oM.Adapters.OpenStreetMap;
using BH.oM.Data.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.OpenStreetMap
{
    public partial class OpenStreetMapAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            if (!(actionConfig is OpenStreetMapConfig))
                actionConfig = new OpenStreetMapConfig();

            if (request is OverpassRequest)
            {
                return Pull(request as OverpassRequest, actionConfig as OpenStreetMapConfig);
            }
            Engine.Reflection.Compute.RecordError("This type of request is not supported.");
            return new List<object>();
        }

        /***************************************************/

        public IEnumerable<object> Pull(OverpassRequest request, OpenStreetMapConfig config)
        {
            GetRequest getRequest = Create.GetRequest(request, config);
            List<object> responses = new List<object>();
            responses.Add(Convert.ToGeospatial(m_HTTPAdapter.Pull(getRequest).First()));

            return responses;
        }

    }
}
