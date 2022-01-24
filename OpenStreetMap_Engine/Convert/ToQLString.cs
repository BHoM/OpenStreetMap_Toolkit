/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using Geosp = BH.oM.Geospatial;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Convert an element to an Overpass API Overpass QL string.")]
        [Input("element", "The element to convert.")]
        [Output("qlString", "Overpass QL string.")]
        public static string ToQLString(this IOpenStreetMapElement element)
        {
            if (element == null) return "";
            string tagFilter = ToQLString(element.KeyValues);
            if (element is Node) return "node" + tagFilter;
            if (element is Way) return "way" + tagFilter;
            if (element is Relation) return "rel" + tagFilter;
            return "";
        }

        /***************************************************/
        [Description("Convert a dictionary of keyValues to an Overpass API Overpass QL tag string.")]
        [Input("keyValues", "The Dictionary to convert.")]
        [Output("qlString", "Overpass QL string.")]
        public static string ToQLString(this Dictionary<string, string> keyValues)
        {
            //other tagFilters to be implemented see: https://wiki.openstreetmap.org/wiki/Overpass_API/Language_Guide#Tag_request_clauses_.28or_.22tag_filters.22.29
            StringBuilder tagFilter = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in keyValues)
            {
                if (kvp.Key == "") continue;
                if (kvp.Value == "")
                {
                    tagFilter.Append(string.Format("[{0}]", kvp.Key));
                }
                else
                {
                    tagFilter.Append(string.Format("[{0}={1}]", kvp.Key, kvp.Value));
                }
            }
            return tagFilter.ToString();
        }

        /***************************************************/
        [Description("Convert a category and subtype to an Overpass API Overpass QL tag string.")]
        [Input("category", "Category is equivalent to OpenStreetMap 'keys'. See https://wiki.openstreetmap.org/wiki/Map_features for full documentation.")]
        [Input("subtype", "Subtypes are equivalent to OpenStreetMap 'values'. See https://wiki.openstreetmap.org/wiki/Map_features for full documentation.")]
        [Output("qlString", "Overpass QL string.")]
        public static string ToQLString(string category, string subtype)
        {
            if (subtype == "")
            {
                return string.Format("[{0}]", category);
            }
            else
            {
                return string.Format("[{0}={1}]", category, subtype);
            }
        }

        /***************************************************/
        [Description("Convert a region to an Overpass API Overpass QL region string.")]
        [Input("region", "The region to convert.")]
        [Output("qlString", "Overpass QL string.")]
        public static string ToQLString(this IOpenStreetMapRegion region)
        {
            if (region is BoundingBox)
            {
                BoundingBox box = region as BoundingBox;
                return string.Format("({0},{1},{2},{3});", box.South, box.West, box.North, box.East);
            }
            if (region is Polygon)
            {
                Polygon polygon = region as Polygon;
                return string.Format("(poly:{0});", polygon.ToLatLonString(" ", "'"));
            }
            if (region is CentreRadius)
            {
                //https://wiki.openstreetmap.org/wiki/Overpass_API/Overpass_QL#Relative_to_other_elements_.28around.29
                CentreRadius circle = region as CentreRadius;
                return string.Format("(around:{0},{1},{2});", circle.Radius, circle.Centre.Latitude, circle.Centre.Longitude);
            }
            if (region is LineStringRadius)
            {
                //https://wiki.openstreetmap.org/wiki/Overpass_API/Overpass_QL#Relative_to_other_elements_.28around.29
                LineStringRadius lineRad = region as LineStringRadius;
                return string.Format("(around:{0},{1});", lineRad.Radius, lineRad.Polygon.ToLatLonString(",", ""));
            }
            if (region is TaggedArea)
            {
                TaggedArea taggedArea = region as TaggedArea;
                string keyValues = ToQLString(taggedArea.KeyValues);
                return string.Format("area{0};", keyValues);
            }
            return "";
        }

        /***************************************************/
        [Description("Convert a IGeospatialRegion to an Overpass API Overpass QL region string.")]
        [Input("region", "The IGeospatialRegion region to convert.")]
        [Output("qlString", "Overpass QL string.")]
        public static string ToQLString(this Geosp.IGeospatialRegion region)
        {
            if (region is Geosp.BoundingBox)
            {
                Geosp.BoundingBox box = region as Geosp.BoundingBox;
                return string.Format("({0},{1},{2},{3});", box.Min.Latitude, box.Min.Longitude, box.Max.Latitude, box.Max.Longitude);
            }
            if (region is Geosp.Polygon)
            {
                Geosp.Polygon polygon = region as Geosp.Polygon;
                Base.Compute.RecordWarning("The outer most polygon has been used for the OpenStreetMap search.");
                return string.Format("(poly:{0});", polygon.Polygons[0].ToLatLonString(" ", "'"));
            }
            if (region is Geosp.Circle)
            {
                //https://wiki.openstreetmap.org/wiki/Overpass_API/Overpass_QL#Relative_to_other_elements_.28around.29
                Geosp.Circle circle = region as Geosp.Circle;
                return string.Format("(around:{0},{1},{2});", circle.Radius, circle.Centre.Latitude, circle.Centre.Longitude);
            }
            if (region is Geosp.LineStringRadius)
            {
                //https://wiki.openstreetmap.org/wiki/Overpass_API/Overpass_QL#Relative_to_other_elements_.28around.29
                Geosp.LineStringRadius lineRad = region as Geosp.LineStringRadius;
                return string.Format("(around:{0},{1});", lineRad.Radius, lineRad.LineString.ToLatLonString(",", ""));
            }
            return "";
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static string ToLatLonString(this Polygon polygon, string separator, string startEndChar)
        {
            string latLonString = "";
            latLonString += startEndChar;
            foreach (Node n in polygon.Nodes)
            {
                latLonString += string.Format("{0}{1}{2}{1}", n.Latitude, separator, n.Longitude);
            }
            latLonString = latLonString.Remove(latLonString.Length - 1);
            latLonString += startEndChar;
            return latLonString;
        }

        /***************************************************/

        private static string ToLatLonString(this Geosp.LineString linestring, string separator, string startEndChar)
        {
            string latLonString = "";
            latLonString += startEndChar;
            foreach (Geosp.Point p in linestring.Points)
            {
                latLonString += string.Format("{0}{1}{2}{1}", p.Latitude, separator, p.Longitude);
            }
            latLonString = latLonString.Remove(latLonString.Length - 1);
            latLonString += startEndChar;
            return latLonString;
        }

        /***************************************************/

    }
}


