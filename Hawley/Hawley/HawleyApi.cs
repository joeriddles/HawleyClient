using System;
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
		private static readonly string url = "https://api.hawleyusa.com/v2.0/";
		private static readonly string apikey = "KEYGOESHERE";

		static void Main()
		{
			var productVariants = GetProductVariants();

			var variantIds = productVariants.Select(pv => pv.VariantId).ToList();
			var products = GetProductsFromVariantsIds(variantIds);

			List<ProductAndVariant> productsAndVariants = CombineProductsAndVariants(products, productVariants);
			WriteProductVariantsToConsole(productsAndVariants);

			WriteProductVariantsToCsv(productVariants);
		}

		private static List<ProductAndVariant> CombineProductsAndVariants(List<Product> products,
			List<ProductVariant> variants)
		{
			return variants.Join(
				products,
				variant => variant.ProductId,
				product => product.ProductId,
				(variant, product) => new ProductAndVariant { Product = product, Variant = variant }).ToList();
		}

		private static List<ProductVariant> GetProductVariants()
		{
			var client = new RestClient(url);
			var request = new RestRequest("Catalog/Products/Variants", Method.GET);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", $"ApiKey {apikey}");
			request.AddParameter("pageStartIndex", "1");
			request.AddParameter("pageSize", "5");

			IRestResponse response = client.Execute(request);

			var jArray = JsonConvert.DeserializeObject <JArray>(response.Content);
			var productVariants = jArray.ToObject<List<ProductVariant>>();
			return productVariants;
		}

		private static List<Product> GetProductsFromVariantsIds(List<string> variantIds)
		{
			var client = new RestClient(url);
			var request = new RestRequest("Catalog/Products", Method.GET);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", $"ApiKey {apikey}");
			request.AddParameter("pageStartIndex", "1");
			request.AddParameter("pageSize", "5");

			IRestResponse response = client.Execute(request);

			var jArray = JsonConvert.DeserializeObject<JArray>(response.Content);
			var products = jArray.ToObject<List<Product>>();
			return products;
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

		private static void WriteProductVariantsToConsole(List<ProductAndVariant> productsAndVariants)
		{
			foreach (var pav in productsAndVariants)
			{
				Console.WriteLine(
					$@"Handle:	{pav.Variant.VariantNo}
Title:	{pav.Product.Name}
Body(HTML):	{pav.Product.Description}
");
			}
		}
	}
}
