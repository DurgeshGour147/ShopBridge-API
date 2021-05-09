using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Jewelry_StoreTests
{
    public class RestServiceUtils
    {
        private static HttpClient CreateClient(string uri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        public static V MakeRestCall<K, V>(K request, string absolutepath, string serviceUri) where K : new()
        {
            V returnval = default(V);
            try
            {
                var restClient = CreateClient(serviceUri);
                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                HttpResponseMessage response = restClient.PostAsync(absolutepath, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = response.Content.ReadAsStringAsync().Result;
                    returnval = JsonConvert.DeserializeObject<V>(responseAsString);
                }
            }
            catch
            {

            }
            return returnval;
        }
        public static V MakeGetRestCall<K, V>(K request, string path, string serviceUri)
        {
            V returnval = default(V);
            try
            {
                var restClient = CreateClient(serviceUri);
                var req = new HttpRequestMessage(HttpMethod.Get, path);
                req.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = restClient.SendAsync(req).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = response.Content.ReadAsStringAsync().Result;
                    returnval = JsonConvert.DeserializeObject<V>(responseAsString);
                }
            }
            catch { }
            return returnval;
        }
    }
}
