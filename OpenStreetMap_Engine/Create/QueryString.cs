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
using BH.oM.Osm;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Osm
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Create an OSM QueryString")]
        [Input("query", "Query as string")]
        [Output("QueryString", "OSM QueryString")]
        public static QueryString QueryString(string query)
        {
            return new QueryString()
            {
                Query = query

            };
        }

        /***************************************************/
        //base uri to overpass api
        private static string jsonBaseUri = "https://www.overpass-api.de/api/interpreter?data=[out:json];";
        [Description("Create an OpenStreetMap_oM QueryString to search for Nodes, Ways and Relations tagged with the specified key and value in a box")]
        [Input("north", "Northern limit of box (maximum decimal latitude)")]
        [Input("south", "Southern limit of box (minimum decimal latitude)")]
        [Input("east", "Eastern limit of box (maximum decimal longitude)")]
        [Input("west", "Western limit of box (minimum decimal longitude)")]
        [Input("key", "OSM tag key for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Input("value", "OSM tag value for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Output("QueryString", "OSM QueryString")]
        public static QueryString QueryInBox(double north, double east, double south, double west, string key, string value)
        {
            //bounding box minimum latitude, minimum longitude, maximum latitude, maximum longitude (or South-West-North-East)

            string q = jsonBaseUri +
                "(" +
                "node[" + key + " = " + value + "]" +
                "(" + south + ", " + west + ", " + north + ", " + east + ");" +
                "way[" + key + " = " + value + "]" +
                "(" + south + ", " + west + ", " + north + ", " + east + ");" +
                "relation[" + key + " = " + value + "]" +
                "(" + south + ", " + west + ", " + north + ", " + east + ");" +
                ");" +
                "(._;" +
                ">;" +
                ");" +
                "out body;";
            return QueryString(q);
        }

        /***************************************************/
        ////too generic to describe
        //public static QueryString QueryForLatLonByCityName(string city, string key, string value)
        //{
            
        //    string q = jsonBaseUri +
        //        "(" +
        //        "node[\"" + key + "\" = \"" + value + "\"][\"name\" = \"" + city + "\"];" +
        //        ");" +
        //        "(._;>;);" +
        //        "out;";
        //    return QueryString(q);
        //}

        ///***************************************************/
        ////too generic to describe
        //public static QueryString QueryForLatLonByNameKeyValue(string name, string key, string value)
        //{
        //    string q = jsonBaseUri +
        //        "(" +
        //        "way[\"" + key + "\" = \"" + value + "\"][\"name\" = \"" + name + "\"];" +
        //        "relation[\"" + key + "\" = \"" + value + "\"][\"name\" = \"" + name + "\"];" +
        //        ");" +
        //        "(._;>;);" +
        //        "out center;";
        //    return QueryString(q);
        //}

        /***************************************************/
        [Description("Create an OpenStreetMap_oM QueryString to search for Ways tagged with the specified key and value within a radius of a point")]
        [Input("radius", "Search radius in metres")]
        [Input("latitude", "Decimal latitude of centre point")]
        [Input("longitude", "Decimal longitudeof centre point")]
        [Input("key", "OSM tag key for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Input("value", "OSM tag value for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Output("QueryString", "OSM QueryString")]
        public static QueryString QueryWayFromPointAndRadiusByKeyValue(double radius, double latitude, double longitude, string key, string value)
        {
            string q = jsonBaseUri +
                "(" +
                "way[" + key + " = " + value + "]" +
                "(around:" + radius + ", " + latitude + ", " + longitude + ");" +
                ");" +
                "(._;>;);" +
                "out body;";
            return QueryString(q);
        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM QueryString to search for Nodes tagged with the specified key within a radius of a point")]
        [Input("radius", "Search radius in metres")]
        [Input("latitude", "Decimal latitude of centre point")]
        [Input("longitude", "Decimal longitudeof centre point")]
        [Input("key", "OSM tag key for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        
        [Output("QueryString", "OSM QueryString")]
        public static QueryString QueryNodesFromPointAndRadiusByKey(double radius, double latitude, double longitude, string key)
        {
            string q = jsonBaseUri +
                "(" +
                "node[" + key + "]" +
                "(around:" + radius + ", " + latitude + ", " + longitude + ");" +
                ");" +
                "out body;";
            return QueryString(q);
        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM QueryString to search for Nodes tagged with the specified key, value within a radius of a point")]
        [Input("radius", "Search radius in metres")]
        [Input("latitude", "Decimal latitude of centre point")]
        [Input("longitude", "Decimal longitudeof centre point")]
        [Input("key", "OSM tag key for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Input("value", "OSM tag value for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Output("QueryString", "OSM QueryString")]
        public static QueryString QueryNodesFromPointAndRadiusByKeyValue(double radius, double latitude, double longitude, string key, string value)
        {
            string q = jsonBaseUri +
                "(" +
                "node[" + key + " = " + value + "]" +
                "(around:" + radius + ", " + latitude + ", " + longitude + ");" +
                ");" +
                "out body;";
            return QueryString(q);
        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM QueryString to search for Nodes, Ways and Relations tagged with the specified key, value within a radius of a point")]
        [Input("radius", "Search radius in metres")]
        [Input("latitude", "Decimal latitude of centre point")]
        [Input("longitude", "Decimal longitudeof centre point")]
        [Input("key", "OSM tag key for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Input("value", "OSM tag value for search. See: https://wiki.openstreetmap.org/wiki/Tags")]
        [Output("QueryString", "OSM QueryString")]
        public static QueryString QueryFromPointAndRadiusKeyValue(double radius, double latitude, double longitude, string key, string value)
        {
            string q = jsonBaseUri +
                "(" +
                "node[" + key + " = " + value + "]" +
                "(around:" + radius + ", " + latitude + ", " + longitude + ");" +
                "way[" + key + " = " + value + "]" +
                "(around:" + radius + ", " + latitude + ", " + longitude + ");" +
                "relation[" + key + " = " + value + "]" +
                "(around:" + radius + ", " + latitude + ", " + longitude + ");" +
                "area[" + key + " = " + value + "]" +
                "(around:" + radius + ", " + latitude + ", " + longitude + ");" +
                ");" +
                "out body;";
            return QueryString(q);
        }

        /***************************************************/

    }
}
