/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using System.Collections.Generic;
using System;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Convert longitude to Universal Transverse Mercator zone.")]
        [Input("longitude", "The longitude to convert, in the range -180.0 to 180.0 with up to 7 decimal places.")]
        [Output("utmZone","Universal transverse Mercator zone.")]
        public static int ToUTMZone(this double longitude)
        {
            return (int)Math.Floor((longitude + 180) / 6) +1;
        }
        /***************************************************/
        [Description("Convert longitude of a Node to Universal Transverse Mercator zone.")]
        [Input("node", "The node to convert.")]
        [Output("utmZone", "Universal transverse Mercator zone.")]
        public static int ToUTMZone(this Node node)
        {
            return ToUTMZone(node.Longitude);
        }
        /***************************************************/
        [Description("Convert all nodes in a Way to single, averaged Universal Transverse Mercator zone.")]
        [Input("way", "The way to convert.")]
        [Output("utmZone", "Universal transverse Mercator zone.")]
        public static int ToUTMZone(this Way way)
        {
            double averageUTM = 0;
            foreach (Node n in way.Nodes)
                averageUTM += n.ToUTMZone();
            return (int)averageUTM / way.Nodes.Count;
        }
        /***************************************************/
        [Description("Convert all nodes in a collection of Ways to single, averaged Universal Transverse Mercator zone.")]
        [Input("ways", "The ways to convert.")]
        [Output("utmZone", "Universal Transverse Mercator zone.")]
        public static int ToUTMZone(this List<Way> ways)
        {
            double averageUTM = 0;
            foreach (Way w in ways)
                averageUTM += w.ToUTMZone();
            return (int)averageUTM / ways.Count;
        }
    }
}


