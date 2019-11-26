/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using BH.oM.OpenStreetMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.OpenStreetMap
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Create an OpenStreetMap Way from nodes and id")]
        [Input("latLonNodes", "List of OpenStreetMap Nodes")]
        [Output("way", "OpenStreetMap Way")]
        public static Way Way(List<Node>latLonNodes, long osmId)
        {
            return new Way()
            {
                Nodes = latLonNodes,

                OsmID = osmId

            };
        }

        /***************************************************/
        [Description("Create an OpenStreetMap Way from Nodes ids and id")]
        [Input("pointIds", "List of OpenStreetMap Nodes ids")]
        [Output("way", "OpenStreetMap Way")]
        public static Way Way(List<Int64> pointIds, long osmId)
        {
            return new Way()
            {
                NodeOsmIds = pointIds,

                OsmID = osmId

            };
        }
        /***************************************************/

        [Description("Create an OpenStreetMap Way from key values")]
        [Input("keyValues", "OpenStreetMap tags associated with the Way")]
        [Output("way", "OpenStreetMap Way")]
        public static Way Way(Dictionary<string,string> keyValues)
        {
            return new Way()
            {
                KeyValues = keyValues
            };
            
        }
        /***************************************************/
    }
}
