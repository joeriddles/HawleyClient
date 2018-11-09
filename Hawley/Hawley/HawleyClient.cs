using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hawley
{
	class HawleyClient
	{
		private static HttpClient client = new HttpClient {BaseAddress = new Uri("https://api.hawleyusa.com/v2.0/")};
		private static readonly string ApiKey = "";

		private static void ConfigClient()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Add("Authorization", $"ApiKey {ApiKey}");
		}

		public async Task<List<Product>> GetProducts()
		{
			ConfigClient();
			HttpResponseMessage response = await client.GetAsync("Catalog/Products?pageStartIndex=1&pageSize=5");

			var jArray = JsonConvert.DeserializeObject<JArray>(response.Content.ToString());
			var products = jArray.ToObject<List<Product>>();
			return products;
		}
	}
}
