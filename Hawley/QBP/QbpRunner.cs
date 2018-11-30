using System.Collections.Generic;
using System.Linq;

namespace QBP
{
	public class QbpRunner
	{
		static void Main()
		{
			QbpClient client = new QbpClient();
			// List<Category> categories = client.GetCategoriesList();
			List<string> productCodeList = client.GetProductCodeList(false);
			client.GetProductsFromProductCodes(productCodeList.Take(100).ToList());
		}
	}
}
