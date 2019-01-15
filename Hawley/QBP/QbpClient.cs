using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace QBP
{
	public class QbpClient
	{
		private readonly string baseUrl = "https://clsdev.qbp.com/api3/";
		private readonly string apiKey = "APIKEY";
		public HttpClient Client { get; }

		public QbpClient()
		{
			HttpClientHandler handler = new HttpClientHandler();
			Client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
			Client.DefaultRequestHeaders.Add("X-QBPAPI-KEY", apiKey);
			Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public Dictionary<string, Category> GetCategories()
		{
			var response = Client.GetAsync("1/category/list").Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				CategoryListResponse categoryListResponse = JsonConvert.DeserializeObject<CategoryListResponse>(content);
				return categoryListResponse.Categories.ToDictionary(category => category.Code, category => category);
			}

			return null;
		}

		public List<string> GetProductCodes(bool includeDiscontinuedProducts = true)
		{
			string get = includeDiscontinuedProducts
				? "1/productcode/list"
				: "1/productcode/list?includeDiscontinued=false";

			var response = Client.GetAsync(get).Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				var productCodeList  = JsonConvert.DeserializeObject<ProductCodeListResponse>(content);
				return productCodeList.Codes;
			}

			return null;
		}

		public List<string> GetProductChanges(DateTime start, DateTime end)
		{
			if (start > end)
				throw new ArgumentException("`start` is after `end`.");

			var response = Client.GetAsync($"1/productchange/list?startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}").Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				var productCodeList = JsonConvert.DeserializeObject<ProductCodeListResponse>(content);
				return productCodeList.Codes;
			}

			return null;
		}

		public Dictionary<string, Product> GetProductsFromProductCodes(IEnumerable<string> productCodes)
		{
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{Client.BaseAddress}/1/product");
			// string contentString = "{" + $"\"codes\":[\"{string.Join("\",\"", productCodes)}\"]" + "}";
			string contentString = "{\"codes\":" + JsonConvert.SerializeObject(productCodes) + "}";
			requestMessage.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
			var response = Client.SendAsync(requestMessage).Result;

			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				ProductResponse productsResponse = JsonConvert.DeserializeObject<ProductResponse>(content);
				return productsResponse.Products.ToDictionary(product => product.Code, product => product);
			}

			return null;
		}

		public List<Inventory> GetInventories(IEnumerable<string> productCodes, IEnumerable<string> warehouseCodes)
		{
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{Client.BaseAddress}/1/inventory");

			string contentString = JsonConvert.SerializeObject(new {warehouseCodes, productCodes});
			requestMessage.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
			var response = Client.SendAsync(requestMessage).Result;

			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				var inventoryResponse = JsonConvert.DeserializeObject<InventoryResponse>(content);
				return inventoryResponse.Inventories;
			}

			return null;
		}

		public List<InventoryChange> GetInventoryChanges()
		{
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{Client.BaseAddress}/1/inventorychange?endDate={DateTime.Now:yyyy-MM-dd HH:mm:ss}");
			var response = Client.SendAsync(requestMessage).Result;

			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				var inventoryResponse = JsonConvert.DeserializeObject<InventoryChangeResponse>(content);
				return inventoryResponse.InventoryChanges;
			}

			return null;
		}

		public void GetImageUrlsFromProducts(Dictionary<string, Product> products)
		{
			var response = Client.GetAsync("1/imageserviceinfo").Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				var imageServiceResponse = JsonConvert.DeserializeObject<ImageServiceResponse>(content);
				string url = imageServiceResponse.ImageUrl;

				foreach (var product in products.Values)
				{
					product.Images.ForEach(
						image => product.ImageUrls.Add($"{url}/prodm/{image}")
					);
				}
			}
		}

		public List<Warehouse> GetWarehouses()
		{
			var response = Client.GetAsync("1/warehouse").Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				WarehouseResponse warehouseResponse = JsonConvert.DeserializeObject<WarehouseResponse>(content);
				return warehouseResponse.Warehouses;
			}

			return null;
		}

		public void AddInventoriesToProducts(Dictionary<string, Product> products, List<Inventory> inventories)
		{
			inventories.ForEach(inventory =>
			{
				if (products.ContainsKey(inventory.Product))
					products[inventory.Product].Inventories.Add(inventory);
				else // This should never be hit
					Console.WriteLine(inventory.Product);
			});
		}

		public void AddCategoriesToProducts(Dictionary<string, Product> products, Dictionary<string, Category> categories)
		{
			foreach (var product in products.Values)
			{
				switch (product.CategoryCodes.Count)
				{
					case 0:
						break;
					case 1:
						if (categories.ContainsKey(product.CategoryCodes[0]))
							product.PrimaryCategory = categories[product.CategoryCodes[0]];
						break;
					case 2:
						if (categories.ContainsKey(product.CategoryCodes[0]))
							product.PrimaryCategory = categories[product.CategoryCodes[0]];
						if (categories.ContainsKey(product.CategoryCodes[1]))
							product.SecondaryCategory = categories[product.CategoryCodes[1]];
						break;
					default:
						Console.WriteLine("above 2 categories");
						if (categories.ContainsKey(product.CategoryCodes[0]))
							product.PrimaryCategory = categories[product.CategoryCodes[0]];
						if (categories.ContainsKey(product.CategoryCodes[1]))
							product.SecondaryCategory = categories[product.CategoryCodes[1]];
						foreach (var categoryCode in product.CategoryCodes.Skip(2))
						{
						if (categories.ContainsKey(categoryCode))
								product.OtherCategories.Add(categories[categoryCode]);
						}
						break;
				}
			}
		}
	}
}
