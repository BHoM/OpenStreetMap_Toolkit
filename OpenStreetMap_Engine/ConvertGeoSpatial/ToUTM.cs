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
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static IGeometry IToUTM(this GeoSp.IGeospatial geospatial, int gridZone = 0)
        {
            return ToUTM(geospatial as dynamic,  gridZone);
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial Point to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static Point ToUTM(this GeoSp.Point geospatial, int gridZone = 0)
        {
            Point utmPoint = BH.Engine.Adapters.OpenStreetMap.Convert.ToUTMPoint(geospatial.Latitude, geospatial.Longitude, gridZone);
            return utmPoint;
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial MultiPoint to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiPoint geospatial, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            ConcurrentBag<Point> points = new ConcurrentBag<Point>();
            Parallel.ForEach(geospatial.Points, n =>
            {
                Point utmPoint = ToUTM(n,  gridZone);
                if (utmPoint != null)
                    points.Add(utmPoint);
            }
            );
            composite.Elements.AddRange(points);
            return composite;
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial LineString to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static Polyline ToUTM(this GeoSp.LineString geospatial, int gridZone = 0)
        {
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, Point> pointDict = new ConcurrentDictionary<int, Point>();
            Parallel.For(0, geospatial.Points.Count, n =>
            {
                Point utmPoint = ToUTM(geospatial.Points[n],  gridZone);
                if(utmPoint != null)
                    pointDict.TryAdd(n, utmPoint);
            }
            );
            List<Point> points = pointDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            Polyline utmPolyline = BH.Engine.Geometry.Create.Polyline(points);
            return utmPolyline;
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial MultiLineString to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiLineString geospatial, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            ConcurrentBag<Polyline> polylines = new ConcurrentBag<Polyline>();
            Parallel.ForEach(geospatial.LineStrings, n =>
            {
                polylines.Add(ToUTM(n,  gridZone));
            }
            );

            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial Polygon to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.Polygon geospatial, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, Polyline> polyDict = new ConcurrentDictionary<int, Polyline>();
            
            Parallel.For(0, geospatial.Polygons.Count, n =>
            {
                polyDict.TryAdd(n, ToUTM(geospatial.Polygons[n],  gridZone));
            }
            );
            List<Polyline> polylines = polyDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial MultiPolygon to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiPolygon geospatial, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, CompositeGeometry> polyDict = new ConcurrentDictionary<int, CompositeGeometry>();

            Parallel.For(0, geospatial.Polygons.Count, n =>
            {
                polyDict.TryAdd(n, ToUTM(geospatial.Polygons[n],  gridZone));

            }
            );
            
            List<CompositeGeometry> polylines = polyDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial BoundingBox to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static BoundingBox ToUTM(this GeoSp.BoundingBox geospatial, int gridZone = 0)
        {
            BoundingBox boundingBox = new BoundingBox();
            boundingBox.Max = ToUTM(geospatial.Max,  gridZone);
            boundingBox.Min = ToUTM(geospatial.Min,  gridZone);
            return boundingBox;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial Feature to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static IGeometry ToUTM(this GeoSp.Feature geospatial, int gridZone = 0)
        {
            return ToUTM(geospatial.Geometry as dynamic,  gridZone);
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial FeatureCollection to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.FeatureCollection geospatial, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            foreach (GeoSp.Feature f in geospatial.Features)
                composite.Elements.Add(ToUTM(f,  gridZone));
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial GeometryCollection to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]
        [Input("convertToUTM", "Optional boolean to override conversion to UTM geometry, allowing generation of the geometry in the WGS coordinate space if required. Default is true.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.GeometryCollection geospatial, int gridZone = 0)
        {
            CompositeGeometry composite = new CompositeGeometry();
            foreach (GeoSp.IGeospatial f in geospatial.Geometries)
                composite.Elements.Add(ToUTM(f as dynamic,  gridZone));
            return composite;
        }

        /***************************************************/
        /****           Private Fallback Method         ****/
        /***************************************************/

        private static IGeometry ToUTM(GeoSp.IGeospatial geospatial, int gridZone = 0)
        {
            Reflection.Compute.RecordError($"Unable to convert {geospatial.GetType()} to Universal Transverse Mercator Coordinates.");
            return null;
        }
    }
}
