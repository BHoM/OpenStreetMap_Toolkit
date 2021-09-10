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
                    break;
                case "LineString":
                    return ToGeoJSONLineString(coordinates);
                case "MultiLineString":
                    break;
                case "GeometryCollection":
                    break;
            }
            return null;
        }

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
        public static IGeoJSON ToGeoJSONPolygon(object coordinates)
        {
            Polygon polygon = new Polygon();
            List<object> coords = coordinates as List<object>;
            if (coords == null)
            {
                Reflection.Compute.RecordError("Polygon coordinates could not be converted.");
                return null;
            }
            List<List<List<double>>> lonlats = new List<List<List<double>>>();
            foreach(object c in coords)
            {
                lonlats.Add(GetLineStringCoords(c));
            }
            polygon.Coordinates = lonlats;
            return polygon;
        }

        public static IGeoJSON ToGeoJSONLineString(object coordinates)
        {
            LineString lineString = new LineString();
            
            lineString.Coordinates = GetLineStringCoords(coordinates);

            return lineString;
        }
        public static IGeoJSON ToGeoJSONMultiPoint(object coordinates)
        {
            MultiPoint point = new MultiPoint();
            List<object> coords = coordinates as List<object>;
            if (coords == null)
            {
                Reflection.Compute.RecordError("MultiPoint coordinates could not be converted.");
                return null;
            }
            List<List<double>> lonlats = new List<List<double>>();
            foreach (object c in coords)
                lonlats.Add(GetPointCoords(c));

            point.Coordinates = lonlats;
            return point;
        }
        public static IGeoJSON ToGeoJSONPoint(object coordinates)
        {
            Point point = new Point();
            point.Coordinates = GetPointCoords(coordinates);
            return point;
        }

        private static List<List<double>> GetLineStringCoords(object coordinates)
        {
            List<object> coords = coordinates as List<object>;
            if (coords == null)
            {
                Reflection.Compute.RecordError("LineString coordinates could not be converted.");
                return null;
            }

            List<List<double>> lonlats = new List<List<double>>();
            foreach (object c in coords)
                lonlats.Add(GetPointCoords(c));
            return lonlats;
        }
        private static List<double> GetPointCoords(object coordinates)
        {
            List<object> coords = coordinates as List<object>;
            if (coords == null)
            {
                Reflection.Compute.RecordError("Point coordinates could not be converted.");
                return null;
            }
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

        //private static object GetCoords(object coordinates)
        //{
        //    List<object> coords = coordinates as List<object>;
        //    if (coords == null)
        //    {
        //        Reflection.Compute.RecordError("Coordinates could not be converted.");
        //        return null;
        //    }
        //    if (coords[0] is double && coords[1] is double)
        //    {
        //        double lat = System.Convert.ToDouble(coords[0]);
        //        double lon = System.Convert.ToDouble(coords[1]);
        //        return new List<double>() { lon, lat };
        //    }
        //    foreach (object c in coords)
        //        return GetCoords(c);


        //}
    }
}
