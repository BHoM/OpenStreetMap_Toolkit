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
using BH.oM.Geometry;
using BH.oM.OpenStreetMap;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;
using System.Collections.Generic;

namespace BH.Engine.OpenStreetMap
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        [Description("Create a Node from latitude, longitude and OpenStreetMap id")]
        [Input("latitude", "Latitude in decimal degrees")]
        [Input("longitude", "Longitude in decimal degrees")]
        [Input("osmId", "OpenStreetMap id")]
        [Output("node", "Node")]
        public static Node Node(double latitude = 0.0, double longitude = 0.0, long osmId = 0)
        {
            Node n = new Node()
            {
                Latitude = latitude,

                Longitude = longitude,

                OsmID = osmId
            };

            return n;
        }

        /***************************************************/

        [Description("Create a Node from key values")]
        [Input("keyValues", "OpenStreetMap tags associated with the Node")]
        [Output("node", "Node")]
        public static Node Node(Dictionary<string, string> keyValues)
        {
            Node node = new Node()
            {
                KeyValues = keyValues
            };
            return node;
        }

        [Description("Create a Node from latitude and longitude")]
        [Input("latitude", "Latitude in decimal degrees")]
        [Input("longitude", "Longitude in decimal degrees")]
        [Output("node", "Node")]
        public static Node Node(double latitude = 0.0, double longitude = 0.0)
        {
            Node node = new Node()
            {
                Latitude = latitude,

                Longitude = longitude,
            };
            return node;
        }

        /***************************************************/
    }
}
