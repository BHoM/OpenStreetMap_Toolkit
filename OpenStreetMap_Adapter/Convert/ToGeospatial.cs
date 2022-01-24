﻿/*
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
using BH.Engine.Base;
using BH.oM.Base;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IGeospatial ToGeospatial(object response)
        {
            //reset those added to the feature collection
            m_AddedWays = new List<long>();
            m_AddedNodes = new List<long>();
            Dictionary<long, Feature> nodes = new Dictionary<long, Feature>();
            Dictionary<long, Feature> ways = new Dictionary<long, Feature>();

            if (response is CustomObject)
            {
                CustomObject r = response as CustomObject;
                if (r.CustomData.ContainsKey("elements"))
                {
                    FeatureCollection featureCollection = new FeatureCollection();
                    List<object> elements = (List<object>)r.CustomData["elements"];

                    foreach (object ele in elements)
                    {
                        try
                        {
                            CustomObject element = (CustomObject)ele;
                            long id = GetId(element, "id");
                            if ((string)element.CustomData["type"] == "node")
                            {
                                Feature feature = ToFeature(element);
                                nodes.Add(id, feature);
                            }
                            if ((string)element.CustomData["type"] == "way")
                            {
                                Feature feature = ToFeature(element, nodes);
                                ways.Add(id, feature);
                            }
                            if ((string)element.CustomData["type"] == "relation")
                                featureCollection.Features.Add(ToFeature(element, ways, nodes));

                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException("Unexpected data format from OpenStreetMap " + e.Message);
                        }
                    }
                    featureCollection.Features.AddRange(RemainingFeatures(ways, nodes));
                    return featureCollection;
                }

            }
            Compute.RecordError("Response was not in the expected format.");
            return null;
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static List<Feature> RemainingFeatures(Dictionary<long, Feature> ways, Dictionary<long, Feature> nodes)
        {
            //checking we are returning all the data from the osm response
            List<Feature> toAdd = new List<Feature>();
            foreach (KeyValuePair<long, Feature> pair in ways)
            {
                if (!m_AddedWays.Contains(pair.Key))
                    toAdd.Add(pair.Value);
            }
            foreach (KeyValuePair<long, Feature> pair in nodes)
            {
                if (!m_AddedNodes.Contains(pair.Key))
                    toAdd.Add(pair.Value);
            }
            return toAdd;
        }

        /***************************************************/

        private static Feature ToFeature(CustomObject customObject)
        {
            if ((string)customObject.CustomData["type"] != "node")
            {
                BH.Engine.Base.Compute.RecordError("The customObject provided could not be converted to a Geospatial point feature.");
                return null;
            }
            //convert OSM node to Feature
            double latitude = (double)customObject.CustomData["lat"];
            double longitude = (double)customObject.CustomData["lon"];

            Feature feature = new Feature();
            if (customObject.CustomData.ContainsKey("tags"))
                feature.Properties = ((CustomObject)customObject.CustomData["tags"]).CustomData;

            Point node = new Point()
            {
                Latitude = latitude,
                Longitude = longitude,
            };
            feature.Geometry = node;
            return feature;
        }

        /***************************************************/

        private static Feature ToFeature(CustomObject customObject, Dictionary<long, Feature> nodes)
        {
            if ((string)customObject.CustomData["type"] != "way")
            {
                BH.Engine.Base.Compute.RecordError("The customObject provided could not be converted to a Geospatial LineString or Polyline feature.");
                return null;
            }
            //convert for an OSM way
            Feature feature = new Feature();
            if (customObject.CustomData.ContainsKey("tags"))
                feature.Properties = ((CustomObject)customObject.CustomData["tags"]).CustomData;

            List<long> ids = GetIds(customObject, "nodes");

            LineString way = new LineString();
            foreach (long id in ids)
            {
                way.Points.Add((Point)nodes[id].Geometry);
                m_AddedNodes.Add(id);
            }
            //check if this is a closed polygon
            if (ids.First().Equals(ids.Last()))
            {
                Polygon p = new Polygon();
                p.Polygons.Add(way);
                feature.Geometry = p;
            }
            else
                feature.Geometry = way;
            return feature;
        }

        /***************************************************/

        private static Feature ToFeature(CustomObject customObject, Dictionary<long, Feature> ways, Dictionary<long, Feature> nodes)
        {
            if ((string)customObject.CustomData["type"] != "relation")
            {
                BH.Engine.Base.Compute.RecordError("The customObject provided could not be converted to a Geospatial MultiPolygon or FeatureCollection feature.");
                return null;
            }
            //Convert for an OSM relation
            Feature feature = new Feature();
            if (customObject.CustomData.ContainsKey("members"))
            {
                if (customObject.CustomData.ContainsKey("tags"))
                {
                    CustomObject tags = (CustomObject)customObject.CustomData["tags"];
                    feature.Properties = tags.CustomData;
                }
                string type = "";
                if (feature.Properties.ContainsKey("type"))
                    type = (string)feature.Properties["type"];

                Dictionary<long, string> nodemembers = new Dictionary<long, string>();
                Dictionary<long, string> waymembers = new Dictionary<long, string>();

                foreach (object member in (List<object>)customObject.CustomData["members"])
                {
                    CustomObject element = (CustomObject)member;
                    long osmid = GetId(element, "ref");
                    string role = "";
                    if (element.CustomData.ContainsKey("role"))
                        role = (string)element.CustomData["role"];

                    if ((string)element.CustomData["type"] == "node")
                        nodemembers.Add(osmid, role);

                    if ((string)element.CustomData["type"] == "way")
                        waymembers.Add(osmid, role);
                }
                if (type == "multipolygon")
                {
                    MultiPolygon multiPolygon = new MultiPolygon();
                    foreach (var pair in waymembers)
                    {
                        if (ways[pair.Key].Geometry is Polygon)
                            multiPolygon.Polygons.Add((Polygon)ways[pair.Key].Geometry);

                        if (ways[pair.Key].Geometry is LineString)
                        {
                            Polygon p = new Polygon();
                            p.Polygons.Add((LineString)ways[pair.Key].Geometry);
                            multiPolygon.Polygons.Add(p);
                        }
                        m_AddedWays.Add(pair.Key);
                    }
                    //todo order inner and outer polygons
                    feature.Geometry = multiPolygon;
                }
                //convert to feature collection
                else
                {
                    FeatureCollection featureCollection = new FeatureCollection();

                    foreach (var pair in nodemembers)
                    {
                        featureCollection.Features.Add(nodes[pair.Key]);
                        m_AddedNodes.Add(pair.Key);
                    }

                    foreach (var pair in waymembers)
                    {
                        featureCollection.Features.Add(ways[pair.Key]);
                        m_AddedWays.Add(pair.Key);
                    }

                    feature.Geometry = featureCollection;
                }

            }
            return feature;
        }

        /***************************************************/

        private static long GetId(CustomObject custom, string idPropName)
        {
            long osmid = 0;
            if (custom.CustomData[idPropName] is int)
                osmid = (int)custom.CustomData[idPropName];
            if (custom.CustomData[idPropName] is long)
                osmid = (long)custom.CustomData[idPropName];
            return osmid;
        }

        /***************************************************/

        private static List<long> GetIds(CustomObject custom, string property)
        {
            List<long> ids = new List<long>();
            if (custom.CustomData.ContainsKey(property))
            {
                foreach (object obj in (List<object>)custom.CustomData[property])
                {
                    if (obj is long)
                        ids.Add((long)obj);
                    if (obj is int)
                        ids.Add((int)obj);
                }
            }
            return ids;
        }

        /***************************************************/
        /****           Private Fields                  ****/
        /***************************************************/

        private static List<long> m_AddedWays { get; set; } = new List<long>();
        private static List<long> m_AddedNodes { get; set; } = new List<long>();
    }
}
