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

using System;
using BH.oM.Osm;
using BH.Engine.Osm;
using BH.oM.HTTP;
using BH.Engine.HTTP;

namespace TestOSM
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryString qs = BH.Engine.Osm.Create.QueryFromPointAndRadiusKeyValue(1000, 53.335087, -6.228350, "amenity", "pub");
            
            string result = BH.Engine.HTTP.Compute.GetRequest(qs.Query);

            OsmObjectContainer osmObjs = BH.Engine.Osm.Create.OsmObjectContainer(result);
        }
    }
}
