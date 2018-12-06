﻿using System;
using System.Collections.Generic;
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

		public List<Category> GetCategoriesList()
		{
			var response = Client.GetAsync("1/category/list").Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				CategoryListResponse categoryListResponse = JsonConvert.DeserializeObject<CategoryListResponse>(content);
				return categoryListResponse.Categories;
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
			var contentString = "{" + $"\"codes\":[\"{string.Join("\",\"", productCodes)}\"]" + "}";
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
	}
}