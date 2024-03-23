using System.Text;
using AMap.Location;
using Newtonsoft.Json;

namespace Amap
{
    public class AmapHttpRequestClient
    {
        protected HttpClient HttpClient { get; }
        public string ApiKey => "a9f4856244b203aea3012964a4664436";

        public AmapHttpRequestClient()
        {
            HttpClient = new HttpClient();
        }

        public async virtual Task<GecodeLocation> PositiveAsync(AmapPositiveHttpRequestParamter requestParamter)
        {
            var client = HttpClient;
            var requestUrlBuilder = new StringBuilder(128);
            requestUrlBuilder.Append("https://restapi.amap.com/v3/geocode/geo");
            requestUrlBuilder.AppendFormat("?key={0}", ApiKey);
            requestUrlBuilder.AppendFormat("&address={0}", requestParamter.Address);
            if (!string.IsNullOrWhiteSpace(requestParamter.City))
            {
                requestUrlBuilder.AppendFormat("&city={0}", requestParamter.City);
            }
            if (!string.IsNullOrWhiteSpace(requestParamter.Output))
            {
                requestUrlBuilder.AppendFormat("&output={0}", requestParamter.Output);
            }
            if (!string.IsNullOrWhiteSpace(requestParamter.Sig))
            {
                requestUrlBuilder.AppendFormat("&sig={0}", requestParamter.Sig);
            }
            requestUrlBuilder.AppendFormat("&batch={0}", requestParamter.Batch);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrlBuilder.ToString());

            var response = await client.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Amap request service returns error! HttpStatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");
            }

            var resultContent = await response.Content.ReadAsStringAsync();
            var amapResponse = JsonConvert.DeserializeObject<AmapPositiveHttpResponse>(resultContent);

            if (!amapResponse.IsSuccess())
            {
                var localizerError = amapResponse.GetErrorMessage();

                throw new Exception($"Resolution address failed:{localizerError}!");
            }
            if (amapResponse.Count <= 0)
            {

                throw new Exception("ResolveLocationZero");
            }

            var locations = amapResponse.Geocodes[0].Location.Split(",");
            var postiveLocation = new GecodeLocation
            {
                Longitude = double.Parse(locations[0]),
                Latitude = double.Parse(locations[1]),
                Level = amapResponse.Geocodes[0].Level
            };
            postiveLocation.AddAdditional("Geocodes", amapResponse.Geocodes);

            return postiveLocation;
        }

        public async virtual Task<ReGeocodeLocation> InverseAsync(AmapInverseHttpRequestParamter requestParamter)
        {

            var client = HttpClient;
            var requestUrlBuilder = new StringBuilder(128);
            requestUrlBuilder.Append("https://restapi.amap.com/v3/geocode/regeo");
            requestUrlBuilder.AppendFormat("?key={0}", ApiKey);
            requestUrlBuilder.AppendFormat("&batch={0}", requestParamter.Batch);
            requestUrlBuilder.AppendFormat("&output={0}", requestParamter.Output);
            requestUrlBuilder.AppendFormat("&radius={0}", requestParamter.Radius);
            requestUrlBuilder.AppendFormat("&extensions={0}", requestParamter.Extensions);
            requestUrlBuilder.AppendFormat("&homeorcorp={0}", requestParamter.HomeOrCorp);
            requestUrlBuilder.AppendFormat("&location=");
            for (int i = 0; i < requestParamter.Locations.Length; i++)
            {
                requestUrlBuilder.AppendFormat("{0},{1}", Math.Round(requestParamter.Locations[i].Longitude, 6),
                    Math.Round(requestParamter.Locations[i].Latitude, 6));
                if (i < requestParamter.Locations.Length - 1)
                {
                    requestUrlBuilder.Append("|");
                }
            }
            if (!string.IsNullOrWhiteSpace(requestParamter.PoiType))
            {
                requestUrlBuilder.AppendFormat("&poitype={0}", requestParamter.PoiType);
            }
            if (!string.IsNullOrWhiteSpace(requestParamter.PoiType))
            {
                requestUrlBuilder.AppendFormat("&poitype={0}", requestParamter.PoiType);
            }
            if (!string.IsNullOrWhiteSpace(requestParamter.Sig))
            {
                requestUrlBuilder.AppendFormat("&sig={0}", requestParamter.Sig);
            }
            if (requestParamter.RoadLevel.HasValue)
            {
                requestUrlBuilder.AppendFormat("&roadlevel={0}", requestParamter.RoadLevel);
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrlBuilder.ToString());

            var response = await client.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Amap request service returns error! HttpStatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");
            }

            var resultContent = await response.Content.ReadAsStringAsync();
            AmapInverseLocationResponse amapResponse = null;
            try
            {
                amapResponse = JsonConvert.DeserializeObject<AmapInverseLocationResponse>(resultContent);

            }
            catch (Exception ex)
            {

                throw new Exception($"Deserialize address failed:{ex}!");
            }

            if (!amapResponse.IsSuccess())
            {
                var localizerError = amapResponse.GetErrorMessage();

                throw new Exception($"Resolution address failed:{localizerError}!");
            }
            var inverseLocation = new ReGeocodeLocation
            {
                Street = amapResponse.Regeocode.AddressComponent.StreetNumber.Street.FirstOrDefault(),
                AdCode = amapResponse.Regeocode.AddressComponent.AdCode,
                Address = amapResponse.Regeocode.Address,
                City = amapResponse.Regeocode.AddressComponent.City,
                Country = amapResponse.Regeocode.AddressComponent.Country,
                District = amapResponse.Regeocode.AddressComponent.District,
                Number = amapResponse.Regeocode.AddressComponent.StreetNumber.Number.FirstOrDefault(),
                Province = amapResponse.Regeocode.AddressComponent.Province,
                Town = amapResponse.Regeocode.AddressComponent.TownShip
            };
            return inverseLocation;
        }

    }
}
