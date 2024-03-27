/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.OpenStreetMap;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Create an OpenStreetMap Node from geographic coordinates.")]
        [Input("latitude", "The latitude of the Node, in the range -90.0 to 90.0 with up to 7 decimal places.")]
        [Input("longitude", "The longitude of the Node, in the range -180.0 to 180.0 with up to 7 decimal places.")]
        [Output("node", "OpenStreetMap Node.")]
        public static Node Node(double latitude, double longitude)
        {
            return new Node()
            {
                Latitude = latitude,
                Longitude = longitude
            };

        }
        /***************************************************/
        [Description("Create an OpenStreetMap Relation for use as a search objective." +
           "See https://wiki.openstreetmap.org/wiki/Map_Features for documentation of all map features.")]
        [Input("keyvalues", "The geographic attributes that you are searching for.")]
        [Output("node", "OpenStreetMap Node.")]
        public static Node Node(Dictionary<string, string> keyvalues)
        {
            return new Node()
            {
                KeyValues = keyvalues
            };
        }
    }
}



