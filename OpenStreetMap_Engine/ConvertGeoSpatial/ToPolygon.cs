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

        [Description("Convert a CustomObject based on a GeoJSON formatted string to BHoM Geospatial Polygon.")]

        public static IGeospatial ToPolygon(CustomObject customObject)
        {
            if (customObject.CheckObject("Polygon"))
            {
                object coordinates = customObject.TopLevelCoordinates();
                if (coordinates != null)
                {
                    Polygon polygon = new Polygon();
                    List<object> coords = GetList(coordinates);
                    if (coords == null)
                        return null;
                    foreach (object c in coords)
                        polygon.Polygons.Add(new LineString() { Points = GetCoordSet(c) });
                    //Todo add check start and end points are equal
                    return polygon;
                }
                    
            }
            return null;
        }

    }
}
