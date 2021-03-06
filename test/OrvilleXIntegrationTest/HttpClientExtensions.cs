﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrvilleXIntegrationTest
{
    public static class HttpClientExtensions
    {
        public static async Task<string> PostResponse(this HttpClient client, string url, Dictionary<string, string> form)
        {
            var c = new FormUrlEncodedContent(form);
            var response = await client.PostAsync(url, c);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<T> GetBody<T>(this HttpClient client, string action, Dictionary<string, string> form)
        {
            var response = await client.PostResponse($"/Test/{action}", form);
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
