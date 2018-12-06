using System.Collections.Generic;
using Newtonsoft.Json;

namespace QBP
{
	public class WarehouseResponse
	{
		public ResponseStatus ResponseStatus { get; set; }
		public List<string> Errors { get; set; }
		public List<Warehouse> Warehouses { get; set; }
	}

	public class InventoryResponse
	{
		public ResponseStatus ResponseStatus { get; set; }
		public List<string> Errors { get; set; }
		public List<Inventory> Inventories { get; set; }
	}

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

	public class ImageServiceResponse
	{
		public string ImageSizes { get; set; }
		public string ImageUrl { get; set; }
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
		// JSON Auto-created properties
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

		public List<string> ImageUrls { get; set; } = new List<string>();
		public List<Inventory> Inventories { get; set; } = new List<Inventory>();

		public static string GetProductHeaders()
		{
			return $"Barcodes,BasePrice,Blocked,Brand,BulletPoints,C,CategoryCodes,ChokingHazardWarningText,ChokingHazardWarningType,Code," +
			       $"Discontinued,Hazmat,Images,IntendedAgeWarningType,ManufacturerPartNumber,MapPrice,Model,MSRP,Name,OrderProcess,ORMD," +
			       $"ProductAttributes,Prop65Text,Recommendations,SeeAlsos,Substitutes,Supersedes,ThirdPartyAllowed,Unit,WeightAndMeasures";
		}

		public override string ToString()
		{
			return $"{string.Join("|", Barcodes)},{BasePrice},{Blocked},{Brand},{string.Join("|", BulletPoints)},{C},{string.Join("|", CategoryCodes)},{ChokingHazardWarningText},{ChokingHazardWarningType}," +
			       $"{Code},{Discontinued},{Hazmat},{string.Join("|", Images)},{IntendedAgeWarningType},{ManufacturerPartNumber},{MapPrice},{Model},{Msrp},{Name}" +
			       $"{OrderProcess},{Ormd},{string.Join("|", ProductAttributes)},{Prop65Text},{string.Join("|", Recommendations)},{string.Join("|", SeeAlsos)}," +
			       $"{string.Join("|", SmallParts)},{string.Join("|", Substitutes)},{string.Join("|", Supersedes)}," +
			       $"{ThirdPartyAllowed},{Unit},{WeightAndMeasures}";
		}
	}

	public class Brand
	{
		public string Code { get; set; }
		public string Name { get; set; }

		public override string ToString()
		{
			return $"{Code}|{Name}";
		}
	}

	public class Model
	{
		public List<string> BulletPoints { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }

		public override string ToString()
		{
			return $"{string.Join("|", BulletPoints)}|{Code}|{Description}|{Name}";
		}
	}

	public class ProductAttribute
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return $"{Name}|{Value}";
		}
	}

	public class WeightAndMeasures
	{
		public Measure Height { get; set; }
		public Measure Length { get; set; }
		public Measure Width { get; set; }
		public Measure Weight { get; set; }

		public override string ToString()
		{
			return $"{Height}|{Length}|{Width}|{Weight}";
		}
	}

	public class Measure
	{
		public string Unit { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return $"{Unit}|{Value}";
		}
	}

	public class Inventory
	{
		public string Product { get; set; }
		public int Quantity { get; set; }
		public string Warehouse { get; set; }
	}

	public class Warehouse
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
	}
}
