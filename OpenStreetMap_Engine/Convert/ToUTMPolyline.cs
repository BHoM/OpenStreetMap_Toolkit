﻿/*
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
using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Convert an OpenStreetMap Way to a UTM Polyline")]
        [Input("way", "OpenStreetMap Way to convert")]
        [Output("polyline", "Converted Way as a Polyline")]
        public static Polyline WayToUTMPolyline(this Way way)
        {
            List<Point> points = new List<Point>();

            foreach (Node n in way.Nodes)
            {
                double[] eastingNorthing = LatLonToUTM(n.Latitude, n.Longitude);

                Point utmPoint = Geometry.Create.Point(eastingNorthing[0], eastingNorthing[1], 0);

                points.Add(utmPoint);
            }

            Polyline polyline = Geometry.Create.Polyline(points);

            return polyline;
        }
    }
}