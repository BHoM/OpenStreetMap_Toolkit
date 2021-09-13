using BH.oM.Adapters.OpenStreetMap.GeoJSON;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OpenStreetMap
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        //Todo make these converts recursive
        public static IGeoJSON IToGeoJSON(string type, object coordinates)
        {
            switch (type)
            {
                case "Point":
                    return ToGeoJSONPoint(coordinates);
                case "MultiPoint":
                    return ToGeoJSONMultiPoint(coordinates);
                case "Polygon":
                    return ToGeoJSONPolygon(coordinates);
                case "MultiPolyon":
                    return ToGeoJSONMultiPolygon(coordinates);
                case "LineString":
                    return ToGeoJSONLineString(coordinates);
                case "MultiLineString":
                    ToGeoJSONMultiLineString(coordinates);
                    break;
                case "GeometryCollection"://todo geometry collection
                    break;
            }
            return null;
        }

        /***************************************************/

        public static IGeoJSON ToGeoJSONBoundingBox(List<object> coordinates)
        {
            BoundingBox boundingBox = new BoundingBox();
            List<double> coords = new List<double>();
            foreach(object c in coordinates)
            {
                if (c is double)
                    coords.Add(System.Convert.ToDouble(c));
            }
            if(coords.Count < 4)
            {
                Reflection.Compute.RecordError("Insufficient coordinates to create a bounding box.");
                return null;
            }
            boundingBox.BBox = coords;
            return boundingBox;
        }

        /***************************************************/

        public static IGeoJSON ToGeoJSONPolygon(object coordinates)
        {
            Polygon polygon = new Polygon();
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            List<List<List<double>>> lonlats = new List<List<List<double>>>();
            foreach(object c in coords)
            {
                lonlats.Add(GetCoordSet(c));
            }
            //Todo add check start and end points are equal
            polygon.Coordinates = lonlats;
            return polygon;
        }

        /***************************************************/

        public static IGeoJSON ToGeoJSONMultiPolygon(object coordinates)
        {
            MultiPolygon polygon = new MultiPolygon();
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            List<List<List<List<double>>>> outer = new List<List<List<List<double>>>>();
            foreach (object c in coords)
            {
                List<object> polyg = GetList(c);
                if (polyg == null)
                    return null;
                List<List<List<double>>> lonlats = new List<List<List<double>>>();
                foreach (object poly in polyg)
                    lonlats.Add(GetCoordSet(poly));

                outer.Add(lonlats);
            }
            //Todo add check start and end points are equal
            polygon.Coordinates = outer;
            return polygon;
        }

        /***************************************************/

        public static IGeoJSON ToGeoJSONLineString(object coordinates)
        {
            LineString lineString = new LineString();
            
            lineString.Coordinates = GetCoordSet(coordinates);

            return lineString;
        }

        /***************************************************/

        public static IGeoJSON ToGeoJSONMultiPoint(object coordinates)
        {
            MultiPoint point = new MultiPoint();
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            List<List<double>> lonlats = new List<List<double>>();
            foreach (object c in coords)
                lonlats.Add(GetPointCoords(c));

            point.Coordinates = lonlats;
            return point;
        }

        /***************************************************/

        public static IGeoJSON ToGeoJSONMultiLineString(object coordinates)
        {
            MultiLineString multiLineString  = new MultiLineString();
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            List<List<List<double>>> lonlats = new List<List<List<double>>>();
            foreach (object c in coords)
            {
                lonlats.Add(GetCoordSet(c));
            }
            //Todo add check start and end points are not equal
            multiLineString.Coordinates = lonlats;
            return multiLineString;
        }

        /***************************************************/

        public static IGeoJSON ToGeoJSONPoint(object coordinates)
        {
            Point point = new Point();
            point.Coordinates = GetPointCoords(coordinates);
            return point;
        }

        /***************************************************/
        /****           Private Methods                  ****/
        /***************************************************/

        private static List<object> GetList(object coordinates)
        {
            List<object> coords = coordinates as List<object>;
            if (coords == null)
            {
                Reflection.Compute.RecordError("Coordinates could not be converted.");
                return null;
            }
            return coords;
        }
        private static List<List<double>> GetCoordSet(object coordinates)
        {
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            List <List<double>> lonlats = new List<List<double>>();
            foreach (object c in coords)
                lonlats.Add(GetPointCoords(c));
            return lonlats;
        }

        /***************************************************/

        private static List<double> GetPointCoords(object coordinates)
        {
            List<object> coords = GetList(coordinates);
            if (coords == null)
                return null;
            if (coords.Count > 2)
                Reflection.Compute.RecordWarning("Point coordinates contains more than 2 values only the first 2 will be used.");
            if (coords.Count < 2)
            {
                Reflection.Compute.RecordError("Insufficient coordinates to create a GeoJSON point.");
                return null;
            }
            double lat = 0;
            double lon = 0;
            try
            {
                lon = System.Convert.ToDouble(coords[0]);
                lat = System.Convert.ToDouble(coords[1]);
            }
            catch
            {
                Reflection.Compute.RecordError("Point coordinates could not be converted to doubles.");
                return null;
            }
            return new List<double>() { lon, lat };
        }
    }
}
