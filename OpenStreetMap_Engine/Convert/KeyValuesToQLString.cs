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
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static string KeyValuesToQLString(Dictionary<string, string> keyvalues)
        {
            
            //other tagfilters to be implmented see: https://wiki.openstreetmap.org/wiki/Overpass_API/Language_Guide#Tag_request_clauses_.28or_.22tag_filters.22.29

            StringBuilder tagfilter = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in keyvalues)
            {
                if (kvp.Key == "") continue;

                if (kvp.Value == "")
                {
                    tagfilter.Append(string.Format("[{0}]", kvp.Key));
                }
                else
                {
                    tagfilter.Append(string.Format("[{0}={1}]", kvp.Key, kvp.Value));
                }

            }

            return tagfilter.ToString();
            
        }
        
    }
}
