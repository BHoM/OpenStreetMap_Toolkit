/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using System;

namespace BH.Engine.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        public static int ToUTMZone(double longitude)
        {
            return (int)Math.Ceiling((longitude + 180) / 6);
        }
        /***************************************************/
        public static int ToUTMZone(this Node node)
        {
            return (int)Math.Ceiling((node.Longitude + 180) / 6);
        }
        /***************************************************/
        public static int ToUTMZone(this Way way)
        {
            double averageUTM = 0;
            foreach (Node n in way.Nodes)
                averageUTM += n.ToUTMZone();
            return (int)averageUTM / way.Nodes.Count;
        }
        /***************************************************/
        public static int ToUTMZone(List<Way> ways)
        {
            double averageUTM = 0;
            foreach (Way w in ways)
                averageUTM += w.ToUTMZone();
            return (int)averageUTM / ways.Count;
        }
    }
}

