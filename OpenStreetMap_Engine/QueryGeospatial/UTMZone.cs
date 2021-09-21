using BH.oM.Geospatial;
using BH.oM.Reflection.Attributes;
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

        [Description("Method for querying an IGeospatial object for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int IUTMZone(IGeospatial geospatial)
        {
            return UTMZone(geospatial as dynamic);
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial object for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this Point geospatial)
        {
            BH.Engine.Adapters.OpenStreetMap.Convert.ToUTMZone(geospatial.Longitude);
            return 0;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiPoint for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this MultiPoint geospatial)
        {
            int zone = 0;
            foreach(Point p in geospatial.Points)
                zone += BH.Engine.Adapters.OpenStreetMap.Convert.ToUTMZone(p.Longitude);
            return (int)zone / geospatial.Points.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial LineString for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this LineString geospatial)
        {
            int zone = 0;
            foreach (Point p in geospatial.Points)
                zone += UTMZone(p);
            return (int)zone / geospatial.Points.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiLineString for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this MultiLineString geospatial)
        {
            int zone = 0;
            foreach (LineString lineString in geospatial.LineStrings)
                zone += UTMZone(lineString);
            return (int)zone / geospatial.LineStrings.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Polygon for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this Polygon geospatial)
        {
            int zone = 0;
            foreach (LineString lineString in geospatial.Polygons)
                zone += UTMZone(lineString);
            return (int)zone / geospatial.Polygons.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiPolygon for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this MultiPolygon geospatial)
        {
            int zone = 0;
            foreach (Polygon polygon in geospatial.Polygons)
                zone += UTMZone(polygon);
            return (int)zone / geospatial.Polygons.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Feature for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this Feature geospatial)
        {
            return UTMZone(geospatial.Geometry); 
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial FeatureCollection for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this FeatureCollection geospatial)
        {
            int zone = 0;
            foreach (Feature feature in geospatial.Features)
                zone += UTMZone(feature);
            return (int)zone / geospatial.Features.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial GeometryCollection for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this GeometryCollection geospatial)
        {
            int zone = 0;
            foreach (IGeospatial feature in geospatial.Geometries)
                zone += UTMZone(feature);
            return (int)zone / geospatial.Geometries.Count;
        }

        /***************************************************/
        /****           Private Fallback Method         ****/
        /***************************************************/
        private static int UTMZone(IGeospatial geospatial)
        {
            Reflection.Compute.RecordWarning("UTM zone could not be found.");
            return 0;
        }
    }
}
