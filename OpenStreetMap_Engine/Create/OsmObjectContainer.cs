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
using BH.oM.Geometry;
using BH.oM.Osm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Osm
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Create an OpenStreetMap_oM OsmObjectContainer from Nodes and Ways")]
        [Input("nodes", "OpenStreetMap_oM Nodes for the container")]
        [Input("ways", "OpenStreetMap_oM Ways for the container")]
        [Output("OsmObjectContainer", "OsmObjectContainer containing the Nodes and Ways")]
        public static OsmObjectContainer OsmObjectContainer(List<Node> nodes, List<Way> ways)
        {
            return new OsmObjectContainer()
            {
               Nodes = nodes,

               Ways = ways
            };

        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM OsmObjectContainer from Nodes")]
        [Input("nodes", "OpenStreetMap_oM Nodes for the container")]
        [Output("OsmObjectContainer", "OsmObjectContainer containing the Nodes")]
        public static OsmObjectContainer OsmObjectContainer(List<Node> nodes)
        {
            return new OsmObjectContainer()
            {
                Nodes = nodes
            };

        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM OsmObjectContainer from Ways")]
        [Input("ways", "OpenStreetMap_oM Ways for the container")]
        [Output("OsmObjectContainer", "OsmObjectContainer with the Ways")]
        public static OsmObjectContainer OsmObjectContainer(List<Way> ways)
        {
            return new OsmObjectContainer()
            {
                Ways = ways
            };

        }

        /***************************************************/
        [Description("Create an OpenStreetMap_oM OsmObjectContainer from JSON formatted query result")]
        [Input("OSMQueryJSONResult", "string formatted as JSON")] 
        [Output("OsmObjectContainer", "OsmObjectContainer containing the objects defined in the JSON formatted query result")]
        public static OsmObjectContainer OsmObjectContainer(string OSMQueryJSONResult)
        {
            List<Way> ways = new List<Way>();

            List<Node> nodes = new List<Node>();

            if (OSMQueryJSONResult == null) return new OsmObjectContainer();

            JObject data;

            using (JsonTextReader reader = new JsonTextReader(new StringReader(OSMQueryJSONResult)))
            {
                data = (JObject)JToken.ReadFrom(reader);
            }

            var ele = data.SelectToken("elements");

            if (ele is JArray)
            {
                foreach (JObject g in ele)
                {
                    if ((string)g.SelectToken("type") == "node")
                    {
                        Point location = Geometry.Create.Point((double)g.SelectToken("lon"), (double)g.SelectToken("lat"), 0);

                        Node node = Create.Node(location, (long)g.SelectToken("id"));
                        
                        //add key values to node
                        var tags = g.SelectToken("tags");

                        if (tags != null)
                        {
                            foreach (JProperty jp in tags)
                            {
                                node.KeyValues.Add(new KeyValuePair<string, string>((string)jp.Name, (string)jp.Value));
                            }
                        }

                        nodes.Add(node);
                    }

                    if ((string)g.SelectToken("type") == "way")
                    {
                        List<Int64> ids = new List<Int64>();
                        string wayid = (string)g.SelectToken("id");
                        Way way = Create.Way(ids, wayid);
                        var n = g.SelectToken("nodes");
                        if (n is JArray)
                        {
                            for (int i = 0; i < n.Count(); i++)
                            {
                                ids.Add((long)n[i]);
                            }
                        }
                        var tags = g.SelectToken("tags");
                        if (tags != null)
                        {
                            foreach (JProperty jp in tags)
                            {
                                way.KeyValues.Add(new KeyValuePair<string, string>((string)jp.Name, (string)jp.Value));
                            }
                            
                        }
                        ways.Add(way);
                    }
                    
                }
            }
            foreach (Way way in ways)
            {
                List<Node> waynodes = new List<Node>();
                foreach (Int64 id in way.NodeOsmIds) waynodes.Add(nodes.Find(x => x.OsmID == id));
                way.Nodes = waynodes;
                
            }
            return Create.OsmObjectContainer(nodes, ways);


        }
    }
}
