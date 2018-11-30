using System.Collections.Generic;
using Newtonsoft.Json;

namespace QBP
{
	public class ProductCodeListResponse
	{
		[JsonProperty("responseStatus")]
		public ResponseStatus ResponseStatus { get; set; }
		[JsonProperty("errors")]
		public List<string> Errors { get; set; }
		[JsonProperty("codes")]
		public List<string> Codes { get; set; }
	}

	public class CategoryListResponse
	{
		[JsonProperty("responseStatus")]
		public ResponseStatus ResponseStatus { get; set; }
		[JsonProperty("errors")]
		public List<string> Errors { get; set; }
		[JsonProperty("categories")]
		public List<Category> Categories { get; set; }
	}

	public class ProductResponse
	{
		[JsonProperty("responseStatus")]
		public ResponseStatus ResponseStatus { get; set; }
		[JsonProperty("errors")]
		public List<string> Errors { get; set; }
		[JsonProperty("products")]
		public List<Product> Products { get; set; }
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

	public class Product
	{
		public List<string> Barcodes { get; set; }
		public string BasePrice { get; set; }
		public string Blocked { get; set; }
		public Brand Brand { get; set; }
		public List<string> BulletPoints { get; set; }
		public List<string> CategoryCodes { get; set; }
		public string ChokingHazardWarningText { get; set; }
		public string ChokingHazardWarningType { get; set; }
		public string Code { get; set; }
		public string Discontinued { get; set; }
		public string Hazmat { get; set; }
		public List<string> Images { get; set; }
		public string C { get; set; }
		public string IntendedAgeWarningType { get; set; }
		public string ManufacturerPartNumber { get; set; }
		public string MapPrice { get; set; }
		public Model Model { get; set; }
		public string Msrp { get; set; }
		public string Name { get; set; }
		public string OrderProcess { get; set; }
		public string Ormd { get; set; }
		public List<ProductAttribute> ProductAttributes { get; set; }
		public string Prop65Text { get; set; }
		public List<string> Recommendations { get; set; }
		public List<string> SeeAlsos { get; set; }
		public List<string> SmallParts { get; set; }
		public List<string> Substitutes { get; set; }
		public List<string> Supersedes { get; set; }
		public string ThirdPartyAllowed { get; set; }
		public string Unit { get; set; }
		public WeightAndMeasures WeightAndMeasures { get; set; }
	}

	public class Brand
	{
		public string Code { get; set; }
		public string Name { get; set; }
	}

	public class Model
	{
		public List<string> BulletPoints { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
	}

	public class ProductAttribute
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class WeightAndMeasures
	{
		public Measure Height { get; set; }
		public Measure Length { get; set; }
		public Measure Width { get; set; }
		public Measure Weight { get; set; }
	}

	public class Measure
	{
		public string Unit { get; set; }
		public string Value { get; set; }
	}

}
