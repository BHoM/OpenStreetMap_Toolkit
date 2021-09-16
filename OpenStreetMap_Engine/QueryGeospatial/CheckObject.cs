using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Geospatial
{
    public static partial class Query
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Check a CustomObject from GeoJSON formatted string is of a specified GeoJSON type.")]
        public static bool CheckObject(this CustomObject customObject, string GeoJSONType)
        {
            string gType = (string)customObject.CustomData["type"];
            if (gType != GeoJSONType)
            {
                Reflection.Compute.RecordWarning($"Object is not a {GeoJSONType}.");
                return false;
            }
            return true;
        }

        /***************************************************/

    }
}
