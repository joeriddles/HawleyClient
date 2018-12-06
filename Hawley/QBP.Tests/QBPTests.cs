using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QBP.Tests
{
	[TestClass]
	public class QbpTests
	{
		[TestMethod]
		public void AddInventoriesToProducts_EqualsInventories()
		{
			QbpClient client = new QbpClient();
			List<string> productCodeList = client.GetProductCodeList(false);
			List<Product> products = client.GetProductsFromProductCodes(productCodeList.Take(20));
			List<Warehouse> warehouses = client.GetWarehouse();
			List<Inventory> inventories = client.GetInventories(productCodeList.Take(20),
				warehouses.Select(warehouse => warehouse.Code));
			client.AddInventoriesToProducts(ref products, inventories);
			
			Assert.AreEqual(inventories.Count, products.SelectMany(product => product.Inventories.Select(inventory => inventory)).Count());
		}
	}
}
