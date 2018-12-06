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
			List<Warehouse> warehouses = client.GetWarehouse();

			List<Category> categories = client.GetCategoriesList();
			List<string> productCodeList = client.GetProductCodeList(false);

			List<Product> products = new List<Product>();
			List<Inventory> inventories = new List<Inventory>();
			int i = 0;
			while (i < 50) // Change this to a lower number for testing
			{
				Console.WriteLine(i);
				products.AddRange(client.GetProductsFromProductCodes(productCodeList.Skip(i).Take(100)));
				inventories.AddRange(client.GetInventories(productCodeList.Skip(i).Take(100), warehouses.Select(warehouse => warehouse.Code)));
				i += 100;
			}

			client.AddInventoriesToProducts(ref products, inventories);
			client.GetImageUrlsFromProducts(products);

			File.WriteAllLines("Products.csv", new[]{Product.GetProductHeaders()});
			File.AppendAllLines("Products.csv", products.Select(product => string.Join(",", product)));
		}
	}
}
