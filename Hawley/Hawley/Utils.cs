using System.Collections.Generic;

namespace Hawley
{
	public class Utils
	{
		public static string Join<T>(List<T> lst)
		{
			if (lst == null)
				return null;
			if (lst.Count == 0)
				return "";
			if (lst.Count == 1)
				return lst[0].ToString();

			string str = "";
			for (int i = 0; i < lst.Count - 1; i++)
			{
				str += $"{lst[i].ToString()}, ";
			}
			str += lst[lst.Count - 1].ToString();
			return str;
		}
	}
}
