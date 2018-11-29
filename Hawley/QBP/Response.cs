using System.Collections.Generic;
using Newtonsoft.Json;

namespace QBP
{
	public class Response
	{
		[JsonProperty("responseStatus")]
		public ResponseStatus ResponseStatus { get; set; }
		[JsonProperty("errors")]
		public List<string> Errors { get; set; }
		[JsonProperty("categories")]
		public List<Category> Categories { get; set; }
	}

	public class Category
	{
		[JsonProperty("code")]
		public string Code { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("parentCode")]
		public string ParentCode { get; set; }
		[JsonProperty("childCodes")]
		public List<string> ChildCodes { get; set; }
	}

	public class ResponseStatus
	{
		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
