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
			while (i < productCodeList.Count) // Change this to a lower number for testing
			{
				Console.WriteLine(i);

				var currentProducts = productCodeList.Skip(i).Take(100).ToList();

				products.AddRange(client.GetProductsFromProductCodes(currentProducts));
				inventories.AddRange(client.GetInventories(currentProducts, warehouses.Select(warehouse => warehouse.Code)));
				i += 100;
			}

			var codes = products.Select(product => product.Code).OrderBy(code => code).ToList();

			client.AddInventoriesToProducts(ref products, inventories);
			client.GetImageUrlsFromProducts(products);

			File.WriteAllLines("Products.csv", new[]{Product.GetProductHeaders()});
			File.AppendAllLines("Products.csv", products.Select(product => string.Join(",", product)));
		}
	}
}
