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
using BH.oM.Geometry;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.oM.Adapters.OpenStreetMap
{
    [Description("A BoundingBox defines a region for searches using the cardinal limits.")]
    public class BoundingBox : BHoMObject, IOpenStreetMapRegion
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        [Description("The maximum Latitude of the BoundingBox, in the range -90.0 to 90.0 with up to 7 decimal places.")]
        public virtual double North { get; set; } = 0.0;

        [Description("The minimum Latitude of the BoundingBox, in the range -90.0 to 90.0 with up to 7 decimal places.")]
        public virtual double South { get; set; } = 0.0;

        [Description("The maximum Longitude of the BoundingBox, in the range -180.0 to 180.0 with up to 7 decimal places.")]
        public virtual double East { get; set; } = 0.0;

        [Description("The minimum Longitude of the BoundingBox, in the range -180.0 to 180.0 with up to 7 decimal places.")]
        public virtual double West { get; set; } = 0.0;

    }
}




