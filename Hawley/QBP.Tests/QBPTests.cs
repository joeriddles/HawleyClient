using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QBP.Tests
{
	[TestClass]
	public class QbpTests
	{
		private readonly string apiKey = "APIKEY";

		[TestMethod]
		public void QbpClient_CreateClient_Success()
		{
			QbpClient client = new QbpClient();
			HttpRequestHeaders clientHeaders = client.Client.DefaultRequestHeaders;
			string clientApiKey =
				clientHeaders.Single(header => header.Key.Equals("X-QBPAPI-KEY")).Value.Single();
			Assert.AreEqual(apiKey, clientApiKey);
		}

		[TestMethod]
		public void QbpClient_GetCategoriesList_Success()
		{
			QbpClient client = new QbpClient();
			Dictionary<string, Category> categories = client.GetCategories();

			Assert.IsTrue(categories.Count > 0);
			Assert.IsInstanceOfType(categories.Values.ToList()[0], typeof(QBP.Category));
		}
	}
}
