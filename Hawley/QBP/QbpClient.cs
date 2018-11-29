using System;
using System.Net.Http;
using System.Net.Http.Headers;
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

		public void GetCategoriesList()
		{
			var response = Client.GetAsync("1/category/list").Result;
			if (response.IsSuccessStatusCode)
			{
				string content = response.Content.ReadAsStringAsync().Result;
				Console.WriteLine(content);
				Response responseObject = JsonConvert.DeserializeObject<Response>(content);
			}
		}
	}
}
