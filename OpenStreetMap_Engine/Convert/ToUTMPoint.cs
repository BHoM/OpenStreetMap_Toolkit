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
using BH.oM.Geometry;
using BH.oM.Adapters.OpenStreetMap;
using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;
using CoordinateSharp;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Convert an OpenStreetMap Node to a Point in Universal Transverse Mercator coordinates.")]
        [Input("node", "OpenStreetMap Node to convert.")]
        [Output("utmPoint", "Converted Node as a Point.")]
        public static Point ToUTMPoint(this Node node)
        {
            return ToUTMPoint(node.Latitude, node.Longitude);
        }
        /***************************************************/
        [Description("Convert a collection of OpenStreetMap Nodes to a collection of Points in Universal Transverse Mercator coordinates using parallel processing.")]
        [Input("nodes", "OpenStreetMap Nodes to convert.")]
        [Output("utmPoints", "Converted Nodes as Points.")]
        public static List<Point> ToUTMPoint(this List<Node> nodes)
        {
            ConcurrentBag<Point> points = new ConcurrentBag<Point>();
            Parallel.ForEach(nodes, node =>
            {
                points.Add(ToUTMPoint(node.Latitude, node.Longitude));
            });
            return points.ToList();
        }
        /***************************************************/
        [Description("Convert latitude and longitude to universal transverse mercator.")]
        [Input("lat", "Decimal latitude.")]
        [Input("lon", "Decimal longitude.")]
        [Output("double []", "Array of two doubles as easting and northing (x,y).")]
        public static Point ToUTMPoint(this double lat, double lon, int gridZone = 0)
        {
            Coordinate c = new Coordinate(lat, lon);
            if (gridZone >= 1 && gridZone <= 60)
                c.Lock_UTM_MGRS_Zone(gridZone);
            Point utmPoint = Geometry.Create.Point(c.UTM.Easting, c.UTM.Northing, 0);
            return utmPoint;
        }
        /***************************************************/
    }
}