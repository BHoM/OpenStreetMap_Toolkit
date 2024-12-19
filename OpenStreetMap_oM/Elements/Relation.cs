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
using BH.oM.Base;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.oM.Adapters.OpenStreetMap
{
    [Description("A relation is a group of elements (Members) used to model logical (and usually local) or geographic relationships between objects.")]
    public class Relation : BHoMObject, IOpenStreetMapElement
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/
        [Description("The unique OpenStreetMap id for the Relation.")]
        public virtual long OsmID { get; set; }

        [Description("The KeyValue tags describing the geographic attributes of this Relation.")]
        public virtual Dictionary<string, string> KeyValues { get; set; } = new Dictionary<string, string>();

        [Description("The IOpenStreetMapElement that define this Relation.")]
        public virtual List<IOpenStreetMapElement> Members { get; set; } = new List<IOpenStreetMapElement>();

    }
}





