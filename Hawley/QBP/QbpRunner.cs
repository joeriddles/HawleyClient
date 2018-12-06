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
			List<Category> categories = client.GetCategoriesList();
			List<string> productCodeList = client.GetProductCodeList(false);

			List<Product> products = new List<Product>();
			int i = 0;
			while (i < productCodeList.Count) // Change this to a lower number for testing
			{
				Console.WriteLine(i);
				products.AddRange(client.GetProductsFromProductCodes(productCodeList.Skip(i).Take(100)));
				i += 100;
			}

			client.GetImageUrlsFromProducts(products);

			File.WriteAllLines("Products.csv", new[]{Product.GetProductHeaders()});
			File.AppendAllLines("Products.csv", products.Select(product => string.Join(",", product)));
		}
	}
}
