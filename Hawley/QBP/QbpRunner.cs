namespace QBP
{
	public class QbpRunner
	{
		static void Main()
		{
			QbpClient client = new QbpClient();
			client.GetCategoriesList();
		}
	}
}
