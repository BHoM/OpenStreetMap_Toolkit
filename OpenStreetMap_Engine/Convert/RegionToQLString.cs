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

namespace BH.Engine.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        public static string RegionToQLString(this IOpenStreetMapRegion region)
        {
            if (region is BoundingBox)
            {
                BoundingBox box = region as BoundingBox;

                return string.Format("({0},{1},{2},{3});", box.South, box.West, box.North, box.East);

            }
            if (region is Polygon)
            {
                Polygon polygon = region as Polygon;

                return string.Format("(poly:{0});", polygon.PolygonToLatLonStringNoComma()); 
            }
            if (region is CentreRadius)
            {
                //https://wiki.openstreetmap.org/wiki/Overpass_API/Overpass_QL#Relative_to_other_elements_.28around.29

                CentreRadius circle = region as CentreRadius;

                return string.Format("(around:{0},{1},{2});", circle.Radius, circle.Centre.Latitude, circle.Centre.Longitude);
            }
            if(region is LineStringRadius)
            {
                //https://wiki.openstreetmap.org/wiki/Overpass_API/Overpass_QL#Relative_to_other_elements_.28around.29

                LineStringRadius lineRad = region as LineStringRadius;

                return string.Format("(around:{0},{1});", lineRad.Radius,lineRad.Polygon.PolygonToLatLonString());
            }
            if(region is TaggedArea)
            {
                TaggedArea taggedArea = region as TaggedArea;

                string keyvalues = KeyValuesToQLString(taggedArea.KeyValues);

                return string.Format("area{0};", keyvalues);
            }
            return "";
        }
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
        private static string PolygonToLatLonString(this Polygon polygon)
        {
            string latlonstring = "";

            foreach (Node n in polygon.Nodes)
            {
                latlonstring += string.Format("{0}, {1},", n.Latitude, n.Longitude);
            }

            return latlonstring.Remove(latlonstring.Length - 1); 
        }
        /***************************************************/
        private static string PolygonToLatLonStringNoComma(this Polygon polygon)
        {
            string latlonstring = "'";

            foreach (Node n in polygon.Nodes)
            {
                latlonstring += string.Format("{0} {1} ", n.Latitude, n.Longitude);
            }
            //remove space
            latlonstring = latlonstring.Trim();
            latlonstring += "'";

            return latlonstring;
        }
    }
    
}
