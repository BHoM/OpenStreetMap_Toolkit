/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.oM.Adapters.OpenStreetMap
{
    [Description("A Way is an ordered list of nodes which normally also has at least one tag or is included within a Relation.")]
    public class Way : BHoMObject, IOpenStreetMapElement
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/
        [Description("The list of nodes that define this Way.")]
        public virtual List<Node>Nodes { get; set; } = new List<Node>();

        [Description("The unique OpenStreetMap id for the Way.")]
        public virtual long OsmID { get; set; }

        [Description("The unique OpenStreetMap ids for the Nodes.")]
        public virtual List<long> NodeOsmIds { get; set; } = new List<long>();

        [Description("The KeyValue tags describing the geographic attributes of this Way.")]
        public virtual Dictionary<string, string> KeyValues { get; set; } = new Dictionary<string, string>();

        /***************************************************/
    }
}





