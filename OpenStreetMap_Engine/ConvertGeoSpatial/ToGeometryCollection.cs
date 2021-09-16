using BH.oM.Geospatial;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BH.Engine.Geospatial
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Convert a CustomObject based on a GeoJSON formatted string to BHoM Geospatial GeometryCollection.")]
        public static IGeospatial ToGeometryCollection(CustomObject customObject)
        {
            GeometryCollection geometryCollection = new GeometryCollection();
            List<object> geometries = (List<object>)customObject.CustomData["geometries"];
            if (geometries == null)
                return null;

            foreach(object g in geometries)
            {
                CustomObject custom = (CustomObject)g;
                geometryCollection.Geometries.Add(ToGeospatial(custom));
            }

            return geometryCollection;
        }
    }
}
