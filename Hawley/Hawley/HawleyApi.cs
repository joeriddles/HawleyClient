using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Hawley
{
	class HawleyApi
	{
		static void Main()
		{
			var products = GetProductVariants("https://api.hawleyusa.com/v2.0/");
			WriteProductVariantsToCsv(products);
		}

		private static List<ProductVariant> GetProductVariants(string url)
		{
			var client = new RestClient(url);
			var request = new RestRequest("Catalog/Products/Variants", Method.GET);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", "ApiKey KEYGOESHERE");

			request.AddParameter("pageStartIndex", "1");
			request.AddParameter("pageSize", "5");

			IRestResponse response = client.Execute(request);

			var jArray = JsonConvert.DeserializeObject <JArray>(response.Content);
			var productVariants = jArray.ToObject<List<ProductVariant>>();
			return productVariants;
		}

		private static void WriteProductVariantsToCsv(List<ProductVariant> productVariants)
		{
			var csv = new StringBuilder();

			csv.AppendLine("Handle,Title,Body(HTML)");

			foreach (var pv in productVariants)
			{
				string title = string.Join(" ", pv.Attributes.SelectMany(
					a => a.Attributes
						.Select(
							at => string.Join(" ", new List<string> {at.Name, at.Value})
					)
				));

				var pvLine = $"{pv.VariantNo}," +
				             $"{title}," +
				             $"{pv.Description.Replace(",", " ")}";
				csv.AppendLine(pvLine);
			}

			File.WriteAllText("..\\..\\..\\..\\HawleyProductVariants.csv", csv.ToString());
		}
	}
}
