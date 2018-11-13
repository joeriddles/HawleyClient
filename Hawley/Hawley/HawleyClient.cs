using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Hawley
{
	public class HawleyClient
	{
		public static void RunHawleyClient()
		{
			HttpClientHandler handler = new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};

			using (var client = new HttpClient(handler))
			{
				client.BaseAddress = new Uri("https://api.hawleyusa.com/");
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", "KEY");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var products = GetAllProducts(client);

				HttpResponseMessage response = client.GetAsync("v2.0/Catalog/Products/Variants?pageStartIndex=1&pageSize=1000").Result;
				if (response.IsSuccessStatusCode)
				{
					List<ProductVariant> variants = new List<ProductVariant>();
					var responseString = response.Content.ReadAsStringAsync().Result;
					variants.AddRange(JsonConvert.DeserializeObject<List<ProductVariant>>(responseString));

					KeyValuePair<string, IEnumerable<string>> xPagination = response.Headers.Single(h => h.Key.Equals("X-Pagination"));
					Console.WriteLine(xPagination.Value.Single());
					Dictionary<string, string> responsePaginationDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(xPagination.Value.Single());
					string nextPage = responsePaginationDictionary["NextPageLink"].Split(new string[] { client.BaseAddress.ToString() }, StringSplitOptions.RemoveEmptyEntries).Single();

					while (response.IsSuccessStatusCode && xPagination.Value != null)
					{
						response = client.GetAsync(nextPage).Result;
						responseString = response.Content.ReadAsStringAsync().Result;

						xPagination = response.Headers.Single(h => h.Key.Equals("X-Pagination"));
						Console.WriteLine(xPagination.Value.Single());
						responsePaginationDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(xPagination.Value.Single());
						nextPage = responsePaginationDictionary["NextPageLink"].Split(new string[] { client.BaseAddress.ToString() }, StringSplitOptions.RemoveEmptyEntries).Single();

						var deserializeJson = JsonConvert.DeserializeObject<List<ProductVariant>>(responseString);
						variants.AddRange(deserializeJson);
					}

				}
				else
				{
					//NOK
				}
			}
		}

		public static List<Product> GetAllProducts(HttpClient client)
		{
			List<Product> products = new List<Product>();
			var response = client.GetAsync("v2.0/Catalog/Products?pageStartIndex=1&pageSize=10").Result;
			var responseString = response.Content.ReadAsStringAsync().Result;
			products.AddRange(JsonConvert.DeserializeObject<List<Product>>(responseString));
			return products;
		}
	}
}
