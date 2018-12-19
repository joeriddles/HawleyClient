using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace QBP
{
	public class QbpRunner
	{
		static void Main()
		{
			QbpClient client = new QbpClient();

			// var changes = client.GetInventoryChanges();

			Dictionary<string, Product> products = new Dictionary<string, Product>();
			List<Inventory> inventories = new List<Inventory>();
			List<Warehouse> warehouses = client.GetWarehouse();
			Dictionary<string, Category> categories = client.GetCategoriesList();

			List<string> productCodeList;
			if (File.Exists("Products.json"))
			{
				List<string> productFileLines = File.ReadAllLines("Products.json").ToList();
				var productsList = JsonConvert.DeserializeObject<List<Product>>(productFileLines.Single());
				products = productsList.ToDictionary(product => product.Code, product => product);

				var now = DateTime.Now;
				var twoDaysAgo = now.AddDays(-2);

				productCodeList = client.GetProductChangeList(twoDaysAgo, now);
				int i = 0;
				while (i < productCodeList.Count) // Change this to a lower number for testing
				{
					Console.WriteLine(i);

					var currentProductCodes = productCodeList.Skip(i).Take(100).ToList();
					var updatedProducts = client.GetProductsFromProductCodes(currentProductCodes);

					foreach (var updatedProduct in updatedProducts.Values)
					{
						if (products.ContainsKey(updatedProduct.Code))
							products[updatedProduct.Code] = updatedProduct;
					}
					i += 100;
				}

				i = 0;
			}
			else
			{
				productCodeList = client.GetProductCodeList(false);
				int i = 0;
				while (i < 500) // Change this to a lower number for testing
				{
					Console.WriteLine(i);

					List<string> currentProductCodes = productCodeList.Skip(i).Take(100).ToList();
					var currentProducts = client.GetProductsFromProductCodes(currentProductCodes);

					foreach (var product in currentProducts.Values)
						products.Add(product.Code, product);

					i += 100;
				}
			}

			List<string> codes = products.Values.Select(product => product.Code).ToList();
			int j = 0;
			while (j < codes.Count)
			{
				var currentProductCodes = codes.Skip(j).Take(100).ToList();
				inventories.AddRange(client.GetInventories(currentProductCodes, warehouses.Select(warehouse => warehouse.Code)));
				j += 100;
			}

			client.AddInventoriesToProducts(products, inventories);
			client.AddCategoriesToProducts(products, categories);
			client.GetImageUrlsFromProducts(products);

			File.WriteAllText("Products.json", JsonConvert.SerializeObject(products.Values));

			// File.WriteAllLines("Products.txt", new[]{Product.GetProductHeaders()});
			// File.AppendAllLines("Products.txt", products.Select(product => string.Join(",", product)));
		}
	}
}
