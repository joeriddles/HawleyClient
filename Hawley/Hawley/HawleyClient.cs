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
		//squash

		private readonly string baseUrl = "https://api.hawleyusa.com/v2.0/";
		private readonly string apiKey = "KEY";
		private readonly HttpClient client;

		public HawleyClient()
		{
			HttpClientHandler handler = new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};
			client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public List<Product> GetAllProducts()
		{
			List<Product> products = new List<Product>();
			var response = client.GetAsync("Catalog/Products?pageStartIndex=1&pageSize=1000").Result;
			if (response.IsSuccessStatusCode)
			{
				var responseString = response.Content.ReadAsStringAsync().Result;
				products.AddRange(JsonConvert.DeserializeObject<List<Product>>(responseString));

				var nextPage = GetNextPage(response);
				while (response.IsSuccessStatusCode && nextPage != null)
				{
					try
					{
						response = client.GetAsync(nextPage).Result;
						responseString = response.Content.ReadAsStringAsync().Result;
						products.AddRange(JsonConvert.DeserializeObject<List<Product>>(responseString));
						nextPage = GetNextPage(response);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}
			}

			return products;
		}

		public List<ProductVariant> GetAllVariants()
		{
			List<ProductVariant> variants = new List<ProductVariant>();
			var response = client.GetAsync("Catalog/Products/Variants?pageStartIndex=1&pageSize=1000").Result;
			if (response.IsSuccessStatusCode)
			{
				var responseString = response.Content.ReadAsStringAsync().Result;
				variants.AddRange(JsonConvert.DeserializeObject<List<ProductVariant>>(responseString));

				KeyValuePair<string, IEnumerable<string>> xPagination = response.Headers.Single(h => h.Key.Equals("X-Pagination"));
				Console.WriteLine(xPagination.Value.Single());
				Dictionary<string, string> responsePaginationDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(xPagination.Value.Single());
				string nextPage = responsePaginationDictionary["NextPageLink"].Split(new[] {client.BaseAddress.ToString()}, StringSplitOptions.RemoveEmptyEntries).Single();

				while (response.IsSuccessStatusCode && nextPage != null)
				{
					try
					{
						response = client.GetAsync(nextPage).Result;
						responseString = response.Content.ReadAsStringAsync().Result;

						var deserializeJson = JsonConvert.DeserializeObject<List<ProductVariant>>(responseString);
						variants.AddRange(deserializeJson);

						xPagination = response.Headers.Single(h => h.Key.Equals("X-Pagination"));
						Console.WriteLine(xPagination.Value.Single());
						responsePaginationDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(xPagination.Value.Single());
						nextPage = string.IsNullOrWhiteSpace(responsePaginationDictionary["NextPageLink"])
							? null
							: responsePaginationDictionary["NextPageLink"].Split(new[] {client.BaseAddress.ToString()}, StringSplitOptions.RemoveEmptyEntries).Single();
					}
					catch (Exception e)
					{
						// TODO: Add more exception handling
						Console.WriteLine(e);
					}
				}
			}
			else
			{
				//NOT OK
			}

			return variants;
		}

		public List<VariantInventories> GetVariantInventoriesFromVariantIds(List<string> variantIds)
		{
			List<VariantInventories> inventories = new List<VariantInventories>();
			var response = client.GetAsync("Catalog/Products/Variants/Inventories?pageStartIndex=1&pageSize=1000").Result;
			if (response.IsSuccessStatusCode)
			{
				var responseString = response.Content.ReadAsStringAsync().Result;
				inventories.AddRange(JsonConvert.DeserializeObject<List<VariantInventories>>(responseString));
				var nextPage = GetNextPage(response);
				while (response.IsSuccessStatusCode && nextPage != null)
				{
					try
					{
						response = client.GetAsync(nextPage).Result;
						responseString = response.Content.ReadAsStringAsync().Result;
						inventories.AddRange(JsonConvert.DeserializeObject<List<VariantInventories>>(responseString));
						nextPage = GetNextPage(response);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}

				}
			}
			else
			{
				// NOT OK
			}

			return inventories;
		}

		public string GetNextPage(HttpResponseMessage response)
		{
			var xPagination = response.Headers.Single(h => h.Key.Equals("X-Pagination"));
			Console.WriteLine(xPagination.Value.Single());
			Dictionary<string, string> responsePaginationDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(xPagination.Value.Single());
			return string.IsNullOrWhiteSpace(responsePaginationDictionary["NextPageLink"])
				? null
				: responsePaginationDictionary["NextPageLink"].Split(new[] { client.BaseAddress.ToString() }, StringSplitOptions.RemoveEmptyEntries).Single();
		}
	}
}
