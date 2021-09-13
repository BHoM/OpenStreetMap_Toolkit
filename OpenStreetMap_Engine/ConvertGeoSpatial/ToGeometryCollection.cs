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

        [Description("Convert geoJSON formatted geometry collection to BHoM Geospatial GeometryCollection.")]
        public static IGeospatial ToGeometryCollection(object geometry)
        {
            GeometryCollection geometryCollection = new GeometryCollection();
            List<object> geometries = GetList(geometry);
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
