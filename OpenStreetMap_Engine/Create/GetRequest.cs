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
    public static partial class Create
    {
        [Description("Create a OpenStreetMap Nominatim GetRequest from a free-form string. " +
            "Free-form queries are processed first left-to-right and then right-to-left if that fails. " +
            "So you may search for pilkington avenue, birmingham as well as for birmingham, pilkington avenue. " +
            "Commas are optional, but improve performance by reducing the complexity of the search." +
            " Special phrases can cause Nominatim to search for particular object types see https://wiki.openstreetmap.org/wiki/Nominatim/Special_Phrases/EN for more details.")]
        [Input("structuredSearch", "Alternative query string format split into several parameters for structured requests. Structured requests are faster but are less robust against alternative OSM tagging schemas.")]
        [Input("outputFormat", "The data format for places found. Default is GeoJson.")]
        [Input("outputDetails", "Details to be included in the returned data of places found. Default is null - all details are excluded.")]
        [Input("resultLimitation", "Limit the results to certain areas or number of results. Default is null - search is limited to 10 results.")]
        [Input("polygonOutput", "Format of the polygon geometry of the places found. Default is None.")]
        [Output("getRequest", "The GetRequest.")]
        public static GetRequest GetRequest(string freeFormQuery, OutputFormat outputFormat = OutputFormat.GeoJson, OutputDetails outputDetails = null, ResultLimitation resultLimitation = null, PolygonOutput polygonOutput = PolygonOutput.None)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("q", freeFormQuery.Replace(" ", "+"));

            SetParameters(ref parameters, outputFormat, outputDetails, resultLimitation, polygonOutput);

            Dictionary<string, object> headers = new Dictionary<string, object>();
            AddUserAgentHeader(ref headers);

            return new GetRequest() 
            { 
                BaseUrl = BaseUriNominatim(),
                Headers = headers,
                Parameters = parameters
            };
        }

        /***************************************************/

        [Description("Create the OpenStreetMap Nominatim GetRequest from a structured string.")]
        [Input("structuredSearch", "Alternative query string format split into several parameters for structured requests. Structured requests are faster but are less robust against alternative OSM tagging schemas.")]
        [Input("outputFormat", "The data format for places found. Default is GeoJson.")]
        [Input("outputDetails", "Details to be included in the returned data of places found. Default is null - all details are excluded.")]
        [Input("resultLimitation", "Limit the results to certain areas or number of results. Default is null - search is limited to 10 results.")]
        [Input("polygonOutput", "Format of the polygon geometry of the places found. Default is None.")]
        [Output("getRequest", "The GetRequest.")]
        public static GetRequest GetRequest(StructuredSearch structuredSearch, OutputFormat outputFormat = OutputFormat.GeoJson, OutputDetails outputDetails = null, ResultLimitation resultLimitation = null, PolygonOutput polygonOutput = PolygonOutput.None)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            structuredSearch.AddToParameters(ref parameters);

            SetParameters(ref parameters, outputFormat, outputDetails, resultLimitation, polygonOutput);

            Dictionary<string, object> headers = new Dictionary<string, object>();
            AddUserAgentHeader(ref headers);

            return new GetRequest()
            {
                BaseUrl = BaseUriNominatim(),
                Headers = headers,
                Parameters = parameters
            };
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static string BaseUriNominatim()
        {
            return "https://nominatim.openstreetmap.org/search?";
        }

        /***************************************************/

        private static void AddUserAgentHeader(ref Dictionary<string,object> headers)
        {
            headers.Add("User-Agent", "BHoM.xyz");
        }

        private static void AddToParameters(this OutputDetails outputDetails, ref Dictionary<string, object> parameters)
        {
            if (outputDetails == null)
                return;

            if (outputDetails.AddressDetails)
                parameters.Add("addressdetails", 1);

            if (outputDetails.NameDetails)
                parameters.Add("namedetails", 1);

            if (outputDetails.ExtraTags)
                parameters.Add("extratags", 1);

        }

        /***************************************************/

        private static void AddToParameters(this ResultLimitation resultLimitation, ref Dictionary<string, object> parameters)
        {
            if (resultLimitation == null)
                return;

            if (resultLimitation.Limit > 50)
            {
                Reflection.Compute.RecordWarning("Maximum result limit is 50, limit set to 50.");
                parameters.Add("limit", 50);
            }   
            else if(resultLimitation.Limit < 0)
            {
                Reflection.Compute.RecordWarning("Result limit cannot be less than 1, limit set to 1.");
                parameters.Add("limit", 1);
            }
            else
                parameters.Add("limit", resultLimitation.Limit);

            if (resultLimitation.CountryCodes.Count > 0)
                parameters.Add("countrycodes", String.Join(",", resultLimitation.CountryCodes));

            if (resultLimitation.BoundingBox != null)
                parameters.Add("viewbox",$"{resultLimitation.BoundingBox.West},{resultLimitation.BoundingBox.South},{resultLimitation.BoundingBox.East},{resultLimitation.BoundingBox.North}");

        }

        /***************************************************/

        private static void AddToParameters(this StructuredSearch structuredSearch, ref Dictionary<string, object> parameters)
        {
            if (structuredSearch == null)
                return;

            if (!String.IsNullOrEmpty(structuredSearch.Country))
                parameters.Add("country", structuredSearch.Country);
            if (!String.IsNullOrEmpty(structuredSearch.County))
                parameters.Add("county", structuredSearch.Country);
            if (!String.IsNullOrEmpty(structuredSearch.City))
                parameters.Add("city", structuredSearch.Country);
            if (!String.IsNullOrEmpty(structuredSearch.State))
                parameters.Add("state", structuredSearch.Country);
            if (!String.IsNullOrEmpty(structuredSearch.PostalCode))
                parameters.Add("postalcode", structuredSearch.Country);
        }

        /***************************************************/

        private static void SetParameters(ref Dictionary<string, object> parameters, OutputFormat outputFormat, OutputDetails outputDetails, ResultLimitation resultLimitation, PolygonOutput polygonOutput)
        {
            parameters.Add("format", outputFormat.ToString().ToLower());
            outputDetails.AddToParameters(ref parameters);

            if (polygonOutput != PolygonOutput.None)
                parameters.Add($"polygon_{polygonOutput.ToString().ToLower()}", 1);

            resultLimitation.AddToParameters(ref parameters);
        }
    }
}
