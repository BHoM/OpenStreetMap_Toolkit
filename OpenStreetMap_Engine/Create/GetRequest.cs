using BH.oM.Adapters.HTTP;
using BH.oM.Adapters.OpenStreetMap.Nominatim;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OpenStreetMap
{
    /***************************************************/
    /****           Public Methods                  ****/
    /***************************************************/
    public static partial class Create
    {
        [Description("Create a OpenStreetMap Nominatim GetRequest from a free-form string. " +
            "Free-form queries are processed first left-to-right and then right-to-left if that fails. " +
            "So you may search for pilkington avenue, birmingham as well as for birmingham, pilkington avenue. " +
            "Commas are optional, but improve performance by reducing the complexity of the search." +
            "Special phrases can cause Nominatim to search for particular object types see https://wiki.openstreetmap.org/wiki/Nominatim/Special_Phrases/EN for more details.")]
        [Input("freeFormQuery", "Free-form query string to search for.")]
        [Input("outputFormat", "The data format for places found. Default is GeoJSON. Use GeoJSON to take advantage of converts to GeoJSON based Geospatial objects in the BHoM.")]
        [Input("outputDetails", "Details to be included in the returned data of places found. Default all details are included.")]
        [Input("resultLimitation", "Limit the results to certain areas or number of results. Default search is limited to 50 results across all countries.")]
        [Input("polygonOutput", "Format of the polygon geometry of the places found. Default is GeoJSON. Use GeoJSON to take advantage of converts to GeoJSON based Geospatial objects in the BHoM.")]
        [Output("getRequest", "The GetRequest.")]
        public static GetRequest GetRequest(string freeFormQuery, OutputFormat outputFormat = OutputFormat.GeoJSON, OutputDetails outputDetails = null, ResultLimitation resultLimitation = null, PolygonOutput polygonOutput = PolygonOutput.GeoJSON)
        {
            if (String.IsNullOrEmpty(freeFormQuery))
            {
                Reflection.Compute.RecordError("The freeFormQuery cannot be null or empty.");
                return null;
            }

            if (outputDetails == null)
                outputDetails = new OutputDetails();
            if (resultLimitation == null)
                resultLimitation = new ResultLimitation();

            GetRequest request = new GetRequest(){ BaseUrl = BaseUriNominatimSearch() };

            request.Parameters.Add("q", freeFormQuery.Replace(" ", "+"));

            SetParameters(ref request, outputFormat, outputDetails, resultLimitation, polygonOutput);

            AddUserAgentHeader(ref request);

            return request;
        }

        /***************************************************/

        [Description("Create the OpenStreetMap Nominatim GetRequest from a StructuredAddressSearch.")]
        [Input("addressSearch", "Query structured by address parameters from a StructuredAddressSearch.")]
        [Input("outputFormat", "The data format for places found. Default is GeoJSON. Use GeoJSON to take advantage of converts to GeoJSON based Geospatial objects in the BHoM.")]
        [Input("outputDetails", "Details to be included in the returned data of places found. Default all details are included.")]
        [Input("resultLimitation", "Limit the results to certain areas or number of results. Default search is limited to 50 results across all countries.")]
        [Input("polygonOutput", "Format of the polygon geometry of the places found. Default is GeoJSON. Use GeoJSON to take advantage of converts to GeoJSON based Geospatial objects in the BHoM.")]
        [Output("getRequest", "The GetRequest.")]
        public static GetRequest GetRequest(StructuredAddressSearch addressSearch, OutputFormat outputFormat = OutputFormat.GeoJSON, OutputDetails outputDetails = null, ResultLimitation resultLimitation = null, PolygonOutput polygonOutput = PolygonOutput.GeoJSON)
        {
            if (addressSearch == null)
            {
                Reflection.Compute.RecordError("The addressSearch cannot be null");
                return null;
            }
            if (outputDetails == null)
                outputDetails = new OutputDetails();
            if (resultLimitation == null)
                resultLimitation = new ResultLimitation();

            GetRequest request = new GetRequest() { BaseUrl = BaseUriNominatimSearch() };

            addressSearch.AddToParameters(ref request);

            SetParameters(ref request, outputFormat, outputDetails, resultLimitation, polygonOutput);

            AddUserAgentHeader(ref request);

            return request;
        }

        /***************************************************/

        [Description("Create the OpenStreetMap Nominatim GetRequest for reverse geocoding from a location specified by latitude and longitude." +
            "Reverse geocoding API does not exactly compute the address for the coordinate it receives. It works by finding the closest suitable OpenStreetMap object and returning its address information. " +
            "This may occasionally lead to unexpected results." +
            "The API returns exactly one result or an error when the coordinate is in an area with no OSM data coverage.")]
        [Input("latitude", "Latitude of a coordinate in WGS84 projection. Permitted range is -90 to 90.")]
        [Input("longitude", "Longitude of a coordinate in WGS84 projection. Permitted range is -180 to 180.")]
        [Input("outputFormat", "The data format for places found. Default is GeoJSON. Use GeoJSON to take advantage of converts to GeoJSON based Geospatial objects in the BHoM.")]
        [Input("outputDetails", "Details to be included in the returned data of places found. Default all details are included.")]
        [Input("resultLimitation", "Limit the results to certain areas or number of results. Default search is zoom detail of 18. Other ResultLimitation parameters are not applicable to reverse geocoding.")]
        [Input("polygonOutput", "Format of the polygon geometry of the places found. Default is GeoJSON. Use GeoJSON to take advantage of converts to GeoJSON based Geospatial objects in the BHoM.")]
        [Output("getRequest", "The GetRequest.")]
        public static GetRequest GetRequest(double latitude, double longitude, OutputFormat outputFormat = OutputFormat.GeoJSON, OutputDetails outputDetails = null, ResultLimitation resultLimitation = null, PolygonOutput polygonOutput = PolygonOutput.GeoJSON)
        {
            if(latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                Reflection.Compute.RecordWarning("The latitude or longitude provided is outside the permitted range. Latitude range is -90 to 90. Longitude range is -180 to 180.");
                return null;
            }
            if (outputDetails == null)
                outputDetails = new OutputDetails();
            if (resultLimitation == null)
                resultLimitation = new ResultLimitation();

            GetRequest request = new GetRequest() { BaseUrl = BaseUriNominatimReverse() };

            request.Parameters.Add("lat", latitude);
            request.Parameters.Add("lon", longitude);

            SetParameters(ref request, outputFormat, outputDetails, resultLimitation, polygonOutput);

            AddUserAgentHeader(ref request);

            return request;
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static string BaseUriNominatimSearch()
        {
            return "https://nominatim.openstreetmap.org/search?";
        }

        /***************************************************/

        private static string BaseUriNominatimReverse()
        {
            return "https://nominatim.openstreetmap.org/reverse?";
        }

        /***************************************************/

        private static void AddUserAgentHeader(ref GetRequest request)
        {
            //to conform with usage policy we provide a user agent in the header see https://operations.osmfoundation.org/policies/nominatim/
            request.Headers.Add("User-Agent", "BHoM.xyz");
        }

        /***************************************************/

        private static void AddToParameters(this OutputDetails outputDetails, ref GetRequest request)
        {
            if (outputDetails == null)
                return;

            if (outputDetails.AddressDetails)
                request.Parameters.Add("addressdetails", 1);

            if (outputDetails.NameDetails)
                request.Parameters.Add("namedetails", 1);

            if (outputDetails.ExtraTags)
                request.Parameters.Add("extratags", 1);

        }

        /***************************************************/

        private static void AddToParameters(this ResultLimitation resultLimitation, ref GetRequest request)
        {
            if (resultLimitation == null)
                return;

            //for reverse calls we can only use Zoom limit
            if(request.BaseUrl.Contains("reverse"))
            {
                if(resultLimitation.Zoom >= 0 && resultLimitation.Zoom <= 18)
                    request.Parameters.Add("zoom", resultLimitation.Zoom);
                else
                    Reflection.Compute.RecordWarning("Zoom level of detail is outside of range (0 to 18), zoom detail is set to 18.");
                return;
            }

            if (resultLimitation.Limit > 50)
            {
                Reflection.Compute.RecordWarning("Maximum result limit is 50, limit set to 50.");
                request.Parameters.Add("limit", 50);
            }   
            else if(resultLimitation.Limit < 0)
            {
                Reflection.Compute.RecordWarning("Result limit cannot be less than 1, limit set to 1.");
                request.Parameters.Add("limit", 1);
            }
            else
                request.Parameters.Add("limit", resultLimitation.Limit);

            if (resultLimitation.CountryCodes.Count > 0)
                request.Parameters.Add("countrycodes", String.Join(",", resultLimitation.CountryCodes));

            if (resultLimitation.BoundingBox != null)
                request.Parameters.Add("viewbox",$"{resultLimitation.BoundingBox.West},{resultLimitation.BoundingBox.South},{resultLimitation.BoundingBox.East},{resultLimitation.BoundingBox.North}");

            if (resultLimitation.ExcludedPlaceIds.Count > 0)
                request.Parameters.Add("exclude_place_ids", String.Join(",", resultLimitation.ExcludedPlaceIds.Select(x => x.ToString()).ToList()));
        }

        /***************************************************/

        private static void AddToParameters(this StructuredAddressSearch structuredSearch, ref GetRequest request)
        {
            if (structuredSearch == null)
                return;

            if (!String.IsNullOrEmpty(structuredSearch.Country))
                request.Parameters.Add("country", structuredSearch.Country);

            if (!String.IsNullOrEmpty(structuredSearch.County))
                request.Parameters.Add("county", structuredSearch.County);

            if (!String.IsNullOrEmpty(structuredSearch.City))
                request.Parameters.Add("city", structuredSearch.City);

            if (!String.IsNullOrEmpty(structuredSearch.State))
                request.Parameters.Add("state", structuredSearch.State);

            if (!String.IsNullOrEmpty(structuredSearch.Street))
                request.Parameters.Add("street", structuredSearch.Street);

            if (!String.IsNullOrEmpty(structuredSearch.PostalCode))
                request.Parameters.Add("postalcode", structuredSearch.PostalCode);
        }

        /***************************************************/

        private static void SetParameters(ref GetRequest request, OutputFormat outputFormat, OutputDetails outputDetails, ResultLimitation resultLimitation, PolygonOutput polygonOutput)
        {
            request.Parameters.Add("format", outputFormat.ToString().ToLower());
            outputDetails.AddToParameters(ref request);

            if (polygonOutput != PolygonOutput.None)
                request.Parameters.Add($"polygon_{polygonOutput.ToString().ToLower()}", 1);

            resultLimitation.AddToParameters(ref request);
        }

        /***************************************************/
    }
}
