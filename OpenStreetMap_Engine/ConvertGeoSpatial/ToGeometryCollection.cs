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

        [Description("Convert a CustomObject based on a geoJSON formatted string to BHoM Geospatial GeometryCollection.")]
        public static IGeospatial ToGeometryCollection(CustomObject customObject)
        {
            GeometryCollection geometryCollection = new GeometryCollection();
            List<object> geometries = (List<object>)customObject.CustomData["geometries"];
            if (geometries == null)
                return null;
            //todo test this with 
            foreach(object g in geometries)
            {
                CustomObject custom = (CustomObject)g;
                string gType = (string)custom.CustomData["type"];
                object coordinates = custom.CustomData["coordinates"];
                geometryCollection.Geometries.Add(ToGeospatial(gType, coordinates));
            }

            return geometryCollection;
        }
    }
}
