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
        [Description("Create an OpenStreetMap_oM ElementContainer from Nodes and Ways")]
        [Input("nodes", "OpenStreetMap_oM Nodes for the container")]
        [Input("ways", "OpenStreetMap_oM Ways for the container")]
        [Output("ElementContainer", "ElementContainer containing the Nodes and Ways")]
        public static ElementContainer ElementContainer(List<Node> nodes, List<Way> ways)
        {
            return new ElementContainer()
            {
               Nodes = nodes,

               Ways = ways
            };

        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM ElementContainer from Nodes")]
        [Input("nodes", "OpenStreetMap_oM Nodes for the container")]
        [Output("ElementContainer", "ElementContainer containing the Nodes")]
        public static ElementContainer ElementContainer(List<Node> nodes)
        {
            return new ElementContainer()
            {
                Nodes = nodes
            };

        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM ElementContainer from Ways")]
        [Input("ways", "OpenStreetMap_oM Ways for the container")]
        [Output("ElementContainer", "ElementContainer with the Ways")]
        public static ElementContainer ElementContainer(List<Way> ways)
        {
            return new ElementContainer()
            {
                Ways = ways
            };

        }

    }
}
