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
				productCodeList = client.GetProductChangeList(now, now);
				int i = 0;
				while (i < productCodeList.Count) // Change this to a lower number for testing
				{
					Console.WriteLine(i);

					var currentProducts = productCodeList.Skip(i).Take(100).ToList();
					var updatedProducts = client.GetProductsFromProductCodes(currentProducts);

					foreach (var updatedProduct in updatedProducts.Values)
					{
						if (products.ContainsKey(updatedProduct.Code))
							products[updatedProduct.Code] = updatedProduct;
					}

					inventories.AddRange(client.GetInventories(
						updatedProducts.Values.Select(product => product.Code),
						warehouses.Select(warehouse => warehouse.Code)));

					i += 100;
				}
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

					inventories.AddRange(client.GetInventories(currentProductCodes, warehouses.Select(warehouse => warehouse.Code)));
					i += 100;
				}
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
