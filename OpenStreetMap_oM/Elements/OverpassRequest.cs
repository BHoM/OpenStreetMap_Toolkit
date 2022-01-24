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
using BH.oM.Base;
using BH.oM.Geospatial;
using BH.oM.Data.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BH.oM.Adapters.OpenStreetMap
{
    public class OverpassRequest : BHoMObject, IRequest
    {
        [Description("OpenStreetMap Category to search for. Categories are equivalent to OpenStreetMap 'keys'. See https://wiki.openstreetmap.org/wiki/Map_features for full documentation.")]
        public virtual string Category { get; set; } = "";

        [Description("Optional OpenStreetMap type to search for. Types are equivalent to OpenStreetMap 'values'. See https://wiki.openstreetmap.org/wiki/Map_features for full documentation." +
            "Default value is \"\" and the search will look for all Subtypes for the provided category.")]
        public virtual string Type { get; set; } = "";

        [Description("Geospatial region to search.")]
        public virtual IGeospatialRegion GeospatialRegion { get; set; } = null;
    }
}
