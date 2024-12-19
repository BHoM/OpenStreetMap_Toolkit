/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using System.ComponentModel;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        [Description("Create the OpenStreetMap query from a region to search within and the elements to search for.")]
        [Input("region", "The region to search.")]
        [Input("elements", "The elements to search for.")]
        [Output("queryBuilder", "The QueryBuilder.")]
        public static QueryBuilder QueryBuilder(IOpenStreetMapRegion region, List<IOpenStreetMapElement> elements)
        {
            StringBuilder q = new StringBuilder();
            q.Append(BaseUri());
            if (region is TaggedArea) q.Append(region.ToQLString());
            q.Append("(");
            q.Append(GetElementAndRegion(elements, region));
            q.Append(");");
            q.Append("(._;");
            q.Append(">;");
            q.Append(");");
            q.Append("out body;");
            return new QueryBuilder()
            {
                QueryString = q.ToString()
            };
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static string BaseUri()
        {
            return "https://www.overpass-api.de/api/interpreter?data=[out:json];";
        }

        /***************************************************/

        private static string GetElementAndRegion(List<IOpenStreetMapElement> elements, IOpenStreetMapRegion region)
        {
            if (elements == null) return "";
            string elementQuery = "";
            string regionQuery = "";
            if (region is TaggedArea) regionQuery = "(area);";
            else regionQuery = region.ToQLString();
            foreach (IOpenStreetMapElement element in elements)
            {
                elementQuery += element.ToQLString() + regionQuery;
            }
            return elementQuery;
        }

        /***************************************************/
    }
}






