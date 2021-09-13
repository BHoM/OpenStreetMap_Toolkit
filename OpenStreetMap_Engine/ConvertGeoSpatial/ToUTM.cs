using BH.oM.Geometry;
using GeoSp = BH.oM.Geospatial;
using CoordinateSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Geospatial
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Interface method for converting an IGeospatial to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static IGeometry IToUTM(this GeoSp.IGeospatial geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            return ToUTM(geospatial as dynamic, convertToUTM, gridZone);
        }

        /***************************************************/

        [Description("Method to convert an IGeospatial Point to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static Point ToUTM(this GeoSp.Point geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            if(!convertToUTM)
                return BH.Engine.Geometry.Create.Point(geospatial.Longitude, geospatial.Latitude, 0);

            // EagerLoad sets which CoordinateSystems are calculated set all to false except UTM_MGRS
            EagerLoad el = new EagerLoad(false);
            el.UTM_MGRS = true;
            el.Extensions.MGRS = false;
            Coordinate c = new Coordinate(geospatial.Latitude, geospatial.Longitude, el);
            if (gridZone >= 1 && gridZone <= 60)
                c.Lock_UTM_MGRS_Zone(gridZone);
            Point utmPoint = BH.Engine.Geometry.Create.Point(c.UTM.Easting, c.UTM.Northing, 0);
            return utmPoint;
        }

        /***************************************************/

        [Description("Method to convert an IGeospatial MultiPoint to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiPoint geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            ConcurrentBag<Point> points = new ConcurrentBag<Point>();
            Parallel.ForEach(geospatial.Points, n =>
            {
                points.Add(ToUTM(n, convertToUTM, gridZone));
            }
            );
            composite.Elements.AddRange(points);
            return composite;
        }

        /***************************************************/

        [Description("Method to convert an IGeospatial LineString to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static Polyline ToUTM(this GeoSp.LineString geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, Point> pointDict = new ConcurrentDictionary<int, Point>();
            Parallel.For(0, geospatial.Points.Count, n =>
            {
                Point utmPoint = ToUTM(geospatial.Points[n], convertToUTM, gridZone);
                pointDict.TryAdd(n, utmPoint);
            }
            );
            List<Point> points = pointDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            Polyline utmPolyline = BH.Engine.Geometry.Create.Polyline(points);
            return utmPolyline;
        }

        /***************************************************/

        [Description("Method to convert an IGeospatial MultiLineString to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiLineString geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            ConcurrentBag<Polyline> polylines = new ConcurrentBag<Polyline>();
            Parallel.ForEach(geospatial.LineStrings, n =>
            {
                polylines.Add(ToUTM(n, convertToUTM, gridZone));
            }
            );

            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method to convert an IGeospatial Polygon to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static CompositeGeometry ToUTM(this GeoSp.Polygon geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, Polyline> polyDict = new ConcurrentDictionary<int, Polyline>();
            
            Parallel.For(0, geospatial.Polygons.Count, n =>
            {
                polyDict.TryAdd(n, ToUTM(geospatial.Polygons[n], convertToUTM, gridZone));
            }
            );
            List<Polyline> polylines = polyDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method to convert an IGeospatial MultiPolygon to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiPolygon geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, CompositeGeometry> polyDict = new ConcurrentDictionary<int, CompositeGeometry>();

            Parallel.For(0, geospatial.Polygons.Count, n =>
            {
                polyDict.TryAdd(n, ToUTM(geospatial.Polygons[n], convertToUTM, gridZone));
            }
            );
            List<CompositeGeometry> polylines = polyDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method to convert an IGeospatial BoundingBox to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static BoundingBox ToUTM(this GeoSp.BoundingBox geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            BoundingBox boundingBox = new BoundingBox();
            boundingBox.Max = ToUTM(geospatial.Max, convertToUTM, gridZone);
            boundingBox.Min = ToUTM(geospatial.Min, convertToUTM, gridZone);
            return boundingBox;
        }

        /**************************************************/

        [Description("Method to convert an IGeospatial Feature to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("geometry", "Converted geometry.")]
        public static IGeometry ToUTM(this GeoSp.Feature geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            return ToUTM(geospatial.Geometry as dynamic, convertToUTM, gridZone);
        }

        /***************************************************/
        /****           Private Fallback Method         ****/
        /***************************************************/

        private static IGeometry ToUTM(GeoSp.IGeospatial geospatial, bool convertToUTM = true, int gridZone = 0)
        {
            return null;
        }
    }
}
