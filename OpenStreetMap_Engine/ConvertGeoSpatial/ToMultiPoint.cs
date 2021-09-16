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

        [Description("Convert a CustomObject based on a GeoJSON formatted string to BHoM Geospatial MultiPoint.")]

        public static IGeospatial ToMultiPoint(CustomObject customObject)
        {
            if (customObject.CheckObject("MultiPoint"))
            {
                object coordinates = customObject.TopLevelCoordinates();
                if (coordinates != null)
                {
                    MultiPoint multiPoint = new MultiPoint();
                    //list of coordinates per point
                    List<object> coords = GetList(coordinates);
                    if (coords == null)
                        return null;

                    foreach (object c in coords)
                        multiPoint.Points.Add(GetPoint(c));

                    return multiPoint;
                }
                 
            }
            return null;
        }

        /***************************************************/
    }
}
