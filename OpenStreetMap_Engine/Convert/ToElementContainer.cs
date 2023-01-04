/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Base.Attributes;
using BH.oM.Base;
using BH.Engine.Serialiser;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Create an OpenStreetMap_oM ElementContainer from JSON formatted query result.")]
        [Input("openStreetMapQueryJSONResult", "string formatted as JSON.")]
        [Output("elementContainer", "ElementContainer containing the objects defined in the JSON formatted query result.")]
        public static ElementContainer ToElementContainer(this string openStreetMapQueryJSONResult)
        {
            return ToElementContainer(new List<CustomObject> { (CustomObject)Serialiser.Convert.FromJson(openStreetMapQueryJSONResult) });
        }
        /***************************************************/
        [Description("Create an OpenStreetMap_oM ElementContainer from HTTP Pull results.")]
        [Input("queryResults", "results from the HTTP_Adapter.")]
        [Output("elementContainer", "ElementContainer containing the objects defined in the results from the HTTP_Adapter.")]
        public static ElementContainer ToElementContainer(this List<CustomObject> queryResults)
        {
            List<Way> ways = new List<Way>();
            List<Node> nodes = new List<Node>();
            List<Relation> relations = new List<Relation>();
            foreach (CustomObject result in queryResults)
            {
                if (result.CustomData.ContainsKey("elements"))
                {
                    List<object> elements = (List<object>)result.CustomData["elements"];// .Cast<CustomObject>();
                    CustomObject tags = new CustomObject();
                    foreach (object ele in elements)
                    {
                        try
                        {
                            CustomObject element = (CustomObject)ele;
                            long id = 0;
                            if (element.CustomData["id"] is int)
                                id = (int)element.CustomData["id"];
                            if (element.CustomData["id"] is long)
                                id = (long)element.CustomData["id"];
                            if (element.CustomData.ContainsKey("tags"))
                                tags = (CustomObject)element.CustomData["tags"];
                            if ((string)element.CustomData["type"] == "node")
                            {
                                double latitude = (double)element.CustomData["lat"];
                                double longitude = (double)element.CustomData["lon"];
                                Node node = new Node()
                                {
                                    Latitude = latitude,
                                    Longitude = longitude,
                                    OsmID = id
                                };
                                node.AddTags(tags);
                                nodes.Add(node);
                            }
                            if ((string)element.CustomData["type"] == "way")
                            {
                                List<object> ids = new List<object>();
                                if (element.CustomData.ContainsKey("nodes"))
                                    ids = (List<object>)element.CustomData["nodes"];
                                Way way = new Way()
                                {
                                    OsmID = id
                                };
                                way.AddTags(tags);
                                way.AddNodeIds(ids);
                                ways.Add(way);
                            }
                            if ((string)element.CustomData["type"] == "relation")
                            {
                                List<object> members = new List<object>();
                                if (element.CustomData.ContainsKey("members"))
                                    members = (List<object>)element.CustomData["members"];
                                Relation relation = new Relation()
                                {
                                    OsmID = id
                                };
                                relation.AddTags(tags);
                                relation.AddMembers(members);
                                relations.Add(relation);
                            }
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException("Unexpected data format from OpenStreetMap " + e.Message);
                        }
                    }
                }
            }
            foreach (Way way in ways)
            {
                List<Node> waynodes = new List<Node>();
                foreach (Int64 id in way.NodeOsmIds) waynodes.Add(nodes.Find(x => x.OsmID == id));
                way.Nodes = waynodes;
            }
            return new ElementContainer()
            {
                Nodes = nodes,
                Ways = ways,
                Relations = relations
            };

        }
        /***************************************************/
        private static void AddTags(this IOpenStreetMapElement element, CustomObject tags)
        {
            if (tags == null) return;
            foreach (var kvp in tags.CustomData)
            {
                if (!element.KeyValues.ContainsKey(kvp.Key))
                    element.KeyValues.Add(kvp.Key, (string)kvp.Value);
            }
        }
        /***************************************************/
        private static void AddNodeIds(this Way way, List<object> ids)
        {
            if (ids == null) return;
            foreach (object id in ids)
            {
                if (id is long)
                    way.NodeOsmIds.Add((long)id);
                if (id is int)
                    way.NodeOsmIds.Add((int)id);
            }
        }
        /***************************************************/
        private static void AddMembers(this Relation relation, List<object> members)
        {
            if (members == null) return;
            foreach (object member in members)
            {
                CustomObject element = (CustomObject)member;
                long osmid = 0;
                if (element.CustomData["ref"] is int)
                    osmid = (int)element.CustomData["ref"];
                if (element.CustomData["ref"] is long)
                    osmid = (long)element.CustomData["ref"];
                string role = "";
                if (element.CustomData.ContainsKey("role"))
                    role = (string)element.CustomData["role"];
                IOpenStreetMapElement openStreetMapElement = null;
                if ((string)element.CustomData["type"] == "node")
                    openStreetMapElement = new Node() { OsmID = osmid };
                if ((string)element.CustomData["type"] == "way")
                    openStreetMapElement = new Way() { OsmID = osmid };
                if ((string)element.CustomData["type"] == "relation")
                    openStreetMapElement = new Relation() { OsmID = osmid };
                if (role != "")
                    openStreetMapElement.KeyValues.Add("role", role);
                relation.Members.Add(openStreetMapElement);
            }
        }
    }
}


