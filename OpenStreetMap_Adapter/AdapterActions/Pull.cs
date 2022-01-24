/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */
using BH.Engine.Adapters.OpenStreetMap;
using BH.oM.Adapter;
using BH.oM.Adapters.HTTP;
using BH.oM.Adapters.OpenStreetMap;
using BH.oM.Data.Requests;
using BH.oM.Geospatial;
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
                Response response = new Response();
                foreach (object r in Pull(request as OverpassRequest, actionConfig as OpenStreetMapConfig))
                {
                    //if we get a feature collection
                    IGeospatial geospatial = Convert.ToGeospatial(r);
                    if (geospatial is FeatureCollection)
                        response.FeatureCollection = geospatial as FeatureCollection;
                    else
                        response.FeatureCollection.Features.Add(new Feature() { Geometry = geospatial });
                }
                return new List<object> { response };

            }
            Engine.Base.Compute.RecordError("This type of request is not supported.");
            return new List<object>();
        }

        /***************************************************/

        public IEnumerable<object> Pull(OverpassRequest request, OpenStreetMapConfig config)
        {
            GetRequest getRequest = Create.GetRequest(request, config);
            List<object> responses = new List<object>();
            responses.Add(m_HTTPAdapter.Pull(getRequest).First());

            return responses;
        }

    }
}
