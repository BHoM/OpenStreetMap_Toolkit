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
using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.OpenStreetMap
{
    [Description("Define configuration settings for pulling OpenStreetMap data using the OpenStreetMap Adapter")]
    public class OpenStreetMapConfig : ActionConfig
    {
        [Description("Elements are the basic components of OpenStreetMap's conceptual data model of the physical world. This will limit the search to some combination of those elements." +
            "The default is to search for all")]
        public virtual ElementsToSearch ElementsToSearch { get; set; } = ElementsToSearch.All;

        [Description("Global endpoint for OpenStreetMap queries. See https://wiki.openstreetmap.org/wiki/Overpass_API#Public_Overpass_API_instances for more detail. Default is Main ( overpass-api.de)." +
            "If the pull request returns the error \"A task was cancelled\" the endpoint may have crashed and choosing an alternative may provide a result.")]
        public virtual OverpassEndpoint OverpassEndpoint { get; set; } = OverpassEndpoint.Main;
    }
}
