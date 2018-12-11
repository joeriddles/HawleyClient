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
		private HttpClient Client { get; }

		public QbpClient()
		{
			HttpClientHandler handler = new HttpClientHandler();
			Client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
			Client.DefaultRequestHeaders.Add("X-QBPAPI-KEY", apiKey);
			Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public Dictionary<string, Category> GetCategoriesList()
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

		public List<string> GetProductCodeList(bool includeDiscontinuedProducts = true)
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

		public List<Product> GetProductsFromProductCodes(IEnumerable<string> productCodes)
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
				return productsResponse.Products;
			}

			return null;
		}

		public List<Inventory> GetInventories(IEnumerable<string> productCodes, IEnumerable<string> warehouseCodes)
		{
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{Client.BaseAddress}/1/inventory");

			// string contentString = "{" + $"\"codes\":[\"{string.Join("\",\"", productCodes)}\"]" + "}";
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

		public void GetImageUrlsFromProducts(List<Product> products)
		{
			var response = Client.GetAsync("1/imageserviceinfo").Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				var imageServiceResponse = JsonConvert.DeserializeObject<ImageServiceResponse>(content);
				string url = imageServiceResponse.ImageUrl;
				products.ForEach(
					product => product.Images.ForEach(
						image => product.ImageUrls.Add($"{url}/prodm/{image}")
					)
				);
			}
		}

		public List<Warehouse> GetWarehouse()
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

		public void AddInventoriesToProducts(ref List<Product> products, List<Inventory> inventories)
		{
			Dictionary<string, Product> productDictionary = products.ToDictionary(product => product.Code, product => product);
			inventories.ForEach(inventory =>
			{
				if (productDictionary.ContainsKey(inventory.Product))
					productDictionary[inventory.Product].Inventories.Add(inventory);
				else // This should never be hit
					Console.WriteLine(inventory.Product);
			});
			products = productDictionary.Values.ToList();
		}

		public void AddCategoriesToProducts(ref List<Product> products, Dictionary<string, Category> categories)
		{
			products.ForEach(product =>
			{
				switch (product.CategoryCodes.Count)
				{
					case 0:
						break;
					case 1:
						product.PrimaryCategory = categories[product.CategoryCodes[0]];
						break;
					case 2:
						product.PrimaryCategory = categories[product.CategoryCodes[0]];
						product.SecondaryCategory = categories[product.CategoryCodes[1]];
						break;
					default:
						Console.WriteLine("above 2 categories");
						break;

				}
			});
		}
	}
}
