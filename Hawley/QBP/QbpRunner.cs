using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QBP
{
	public class QbpRunner
	{
		static void Main()
		{
			QbpClient client = new QbpClient();

			List<Product> products = new List<Product>();
			List<Inventory> inventories = new List<Inventory>();
			List<Warehouse> warehouses = client.GetWarehouse();
			Dictionary<string, Category> categories = client.GetCategoriesList();

			List<string> productCodeList;
			if (File.Exists("Products.csv"))
			{
				List<string> productFileLines = File.ReadAllLines("Products.csv").ToList();
				productFileLines.RemoveAt(0); // Removes header line

				var now = DateTime.Now;
				productCodeList = client.GetProductChangeList(now, now);
			}
			else
			{
				productCodeList = client.GetProductCodeList(false);
				int i = 0;
				while (i < productCodeList.Count) // Change this to a lower number for testing
				{
					Console.WriteLine(i);

					var currentProducts = productCodeList.Skip(i).Take(100).ToList();

					products.AddRange(client.GetProductsFromProductCodes(currentProducts));
					inventories.AddRange(client.GetInventories(currentProducts, warehouses.Select(warehouse => warehouse.Code)));
					i += 100;
				}
			}

			client.AddInventoriesToProducts(products, inventories);
			client.AddCategoriesToProducts(products, categories);
			client.GetImageUrlsFromProducts(products);

			File.WriteAllLines("Products.csv", new[]{Product.GetProductHeaders()});
			File.AppendAllLines("Products.csv", products.Select(product => string.Join(",", product)));
		}
	}
}
