using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.QueryGeospatial
{
    public static partial class Query
    {
        public static int IUTMZone(IGeospatial geospatial)
        {
            return UTMZone(geospatial as dynamic);
        }

        private static int UTMZone(Point geospatial)
        {
            BH.Engine.Adapters.OpenStreetMap.Convert.ToUTMZone(geospatial.Longitude);
            return 0;
        }

        private static int UTMZone(MultiPoint geospatial)
        {
            int zone = 0;
            foreach(Point p in geospatial.Points)
                BH.Engine.Adapters.OpenStreetMap.Convert.ToUTMZone(p.Longitude);
            return (int)zone / geospatial.Points.Count;
        }

        private static int UTMZone(LineString geospatial)
        {
            int zone = 0;
            foreach (Point p in geospatial.Points)
                UTMZone(p);
            return (int)zone / geospatial.Points.Count;
        }

        private static int UTMZone(MultiLineString geospatial)
        {
            int zone = 0;
            foreach (LineString lineString in geospatial.LineStrings)
                UTMZone(lineString);
            return (int)zone / geospatial.LineStrings.Count;
        }

        private static int UTMZone(Polygon geospatial)
        {
            int zone = 0;
            foreach (LineString lineString in geospatial.Polygons)
                UTMZone(lineString);
            return (int)zone / geospatial.Polygons.Count;
        }

        private static int UTMZone(MultiPolygon geospatial)
        {
            int zone = 0;
            foreach (Polygon polygon in geospatial.Polygons)
                UTMZone(polygon);
            return (int)zone / geospatial.Polygons.Count;
        }

        private static int UTMZone(IGeospatial geospatial)
        {
            Reflection.Compute.RecordWarning("UTM zone could not be found.");
            return 0;
        }
    }
}
