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
		private static readonly string apikey = "KEY";

		static void Main()
		{
			var client = new HawleyClient();

			var categories = GetCategories();
			var flatCategories = FlattenCategories(categories);

			var products = client.GetAllProducts();
			CombineProductAndCategoryNames(ref products, flatCategories);

			var productVariants = client.GetAllVariants();
			var variantIds = productVariants.Select(pv => pv.VariantId).ToList();
			var inventories = client.GetVariantInventoriesFromVariantIds(variantIds);
			productVariants = CombineVariantsAndInventories(productVariants, inventories);


			List<ProductAndVariant> productsAndVariants = CombineProductsAndVariants(products, productVariants);

			WriteProductVariantsToConsole(productsAndVariants);
			WriteProductVariantsToCsv(productVariants);

			Console.ReadLine();
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

		private static List<ProductVariant> CombineVariantsAndInventories(List<ProductVariant> variants,
			List<VariantInventories> inventories)
		{
			var combined = variants.Join(
				inventories,
				variant => variant.VariantId,
				inventory => inventory.VariantId,
				(variant, inventory) => new { variant, inventory });

			var newVariants = new List<ProductVariant>();
			foreach (var c in combined)
			{
				c.variant.Inventories.Add(c.inventory);
				newVariants.Add(c.variant);
			}
			return newVariants;
		}

		private static List<ProductVariant> GetProductVariants(int numberProductVariants)
		{
			var client = new RestClient(url);

			List<ProductVariant> productVariants = new List<ProductVariant>();

			var request = new RestRequest("Catalog/Products/Variants", Method.GET);
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", $"ApiKey {apikey}");
			request.AddParameter("pageStartIndex", 1);

			/*
			if (numberProductVariants > 100)
				request.AddParameter("pageSize", 1000);
			else
				request.AddParameter("pageSize", numberProductVariants);
			*/
			request.AddParameter("pageSize", 5000);

			IRestResponse response = client.Execute(request);
			var jArray = JsonConvert.DeserializeObject <JArray>(response.Content);
			productVariants.AddRange(jArray.ToObject<IEnumerable<ProductVariant>>());

			Parameter responseXPagination = response.Headers.SingleOrDefault(h => h.Name.Equals("X-Pagination"));
			while (responseXPagination?.Value != null)
			{
				Dictionary<string, string> responsePaginationDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseXPagination.Value.ToString());
				string nextPage = responsePaginationDictionary["NextPageLink"].Split(new string[] {url}, StringSplitOptions.RemoveEmptyEntries).Single();
				Console.WriteLine(nextPage);

				request = new RestRequest(nextPage);
				request.AddHeader("Accept", "application/json");
				request.AddHeader("Authorization", $"ApiKey {apikey}");
				response = client.Execute(request);

				jArray = JsonConvert.DeserializeObject<JArray>(response.Content);
				productVariants.AddRange(jArray.ToObject<IEnumerable<ProductVariant>>());

				responseXPagination = response.Headers.SingleOrDefault(h => h.Name.Equals("X-Pagination"));
			}

			return productVariants;
		}

		private static List<Product> GetProductsFromVariantsIds(List<string> variantIds)
		{
			var client = new RestClient(url);
			var request = new RestRequest("Catalog/Products", Method.GET);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", $"ApiKey {apikey}");
			request.AddParameter("variantIds", string.Join(",", variantIds));

			IRestResponse response = client.Execute(request);

			var jArray = JsonConvert.DeserializeObject<JArray>(response.Content);
			var products = jArray.ToObject<List<Product>>();
			return products;
		}

		private static List<VariantInventories> GetVariantInventoriesFromVariantIds(List<string> variantIds)
		{
			var client = new RestClient(url);
			var request = new RestRequest("Catalog/Products/Variants/Inventories", Method.GET);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", $"ApiKey {apikey}");
			request.AddParameter("variantIds", string.Join(",", variantIds));

			IRestResponse response = client.Execute(request);

			var jArray = JsonConvert.DeserializeObject<JArray>(response.Content);
			var inventories = jArray.ToObject<List<VariantInventories>>();
			return inventories;
		}

		private static List<Categories> GetCategories()
		{
			var client = new RestClient(url);
			var request = new RestRequest("Catalog/Categories", Method.GET);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", $"ApiKey {apikey}");

			IRestResponse response = client.Execute(request);

			var jArray = JsonConvert.DeserializeObject<JArray>(response.Content);
			var categories = jArray.ToObject<List<Categories>>();
			return categories;
		}

		private static Dictionary<string, string> FlattenCategories(List<Categories> categories)
		{
			Dictionary<string, string> allCategories = new Dictionary<string, string>();
			categories.ForEach(c => allCategories.Add(c.CategoryId, c.CategoryName));
			categories.ForEach(c => c.SubCategories.ForEach(sc => allCategories.Add(sc.CategoryId, sc.CategoryName)));
			return allCategories;
		}

		private static void CombineProductAndCategoryNames(ref List<Product> products,
			Dictionary<string, string> categories)
		{
			foreach (var product in products)
			{
				foreach (var categoryId in product.CategoryIds)
				{
					if (categories.ContainsKey(categoryId))
						product.CategoryNames.Add(categories[categoryId]);
					else
						product.CategoryNames.Add("");
				}	
			}
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
