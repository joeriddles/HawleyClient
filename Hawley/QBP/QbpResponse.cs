using System.Collections.Generic;
using Newtonsoft.Json;

namespace QBP
{
	public class WarehouseResponse
	{
		public ResponseStatus ResponseStatus { get; set; }
		public List<Error> Errors { get; set; }
		public List<Warehouse> Warehouses { get; set; }
	}

	public class InventoryResponse
	{
		public ResponseStatus ResponseStatus { get; set; }
		public List<Error> Errors { get; set; }
		public List<Inventory> Inventories { get; set; }
	}

	public class ProductCodeListResponse
	{
		public ResponseStatus ResponseStatus { get; set; }
		public List<Error> Errors { get; set; }
		public List<string> Codes { get; set; }
	}

	public class CategoryListResponse
	{
		public ResponseStatus ResponseStatus { get; set; }
		public List<Error> Errors { get; set; }
		public List<Category> Categories { get; set; }
	}

	public class ProductResponse
	{
		public ResponseStatus ResponseStatus { get; set; }
		public List<Error> Errors { get; set; }
		public List<Product> Products { get; set; }
	}

	public class ImageServiceResponse
	{
		public string ImageSizes { get; set; }
		public string ImageUrl { get; set; }
	}

	public class Category
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string ParentCode { get; set; }
		public List<string> ChildCodes { get; set; }
	}

	public class ResponseStatus
	{
		public string Type { get; set; }
		public string ErrorCode { get; set; }
		public string Meta { get; set; }
	}

	public class Error
	{
		public string Details { get; set; }
		public string ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
		public string LogKey { get; set; }
	}

	public class Product
	{
		// JSON Auto-created properties
		public List<string> Barcodes { get; set; }
		public decimal BasePrice { get; set; }
		public bool Blocked { get; set; }
		public Brand Brand { get; set; }
		public List<string> BulletPoints { get; set; }
		public List<string> CategoryCodes { get; set; }
		public string ChokingHazardWarningText { get; set; }
		public string ChokingHazardWarningType { get; set; }
		public string Code { get; set; }
		public bool Discontinued { get; set; }
		public bool Hazmat { get; set; }
		public List<string> Images { get; set; }
		public string C { get; set; }
		public string IntendedAgeWarningText { get; set; }
		public string IntendedAgeWarningType { get; set; }
		public string ManufacturerPartNumber { get; set; }
		public decimal MapPrice { get; set; }
		public Model Model { get; set; }
		public decimal Msrp { get; set; }
		public string Name { get; set; }
		public string OrderProcess { get; set; }
		public bool Ormd { get; set; }
		public List<ProductAttribute> ProductAttributes { get; set; }
		public string Prop65Text { get; set; }
		public List<string> Recommendations { get; set; }
		public List<string> SeeAlsos { get; set; }
		public List<string> SmallParts { get; set; }
		public List<string> Substitutes { get; set; }
		public List<string> Supersedes { get; set; }
		public bool ThirdPartyAllowed { get; set; }
		public string Unit { get; set; }
		public WeightsAndMeasures WeightsAndMeasures { get; set; }

		[JsonIgnore]
		public List<string> ImageUrls { get; set; } = new List<string>();
		[JsonIgnore]
		public List<Inventory> Inventories { get; set; } = new List<Inventory>();
		[JsonIgnore]
		public Category PrimaryCategory { get; set; }
		[JsonIgnore]
		public Category SecondaryCategory { get; set; }

		public static string GetProductHeaders()
		{
			return $"Barcodes,BasePrice,Blocked,Brand,BulletPoints,C,CategoryCodes,ChokingHazardWarningText,ChokingHazardWarningType,Code," +
			       $"Discontinued,Hazmat,Images,IntendedAgeWarningText,IntendedAgeWarningType,ManufacturerPartNumber,MapPrice,Model,MSRP,Name,OrderProcess,ORMD," +
			       $"ProductAttributes,Prop65Text,Recommendations,SeeAlsos,Substitutes,Supersedes,ThirdPartyAllowed,Unit,WeightsAndMeasures";
		}

		public override string ToString()
		{
			return $"{string.Join("|", Barcodes)},{BasePrice},{Blocked},{Brand},{string.Join("|", BulletPoints)},{C},{string.Join("|", CategoryCodes)},{ChokingHazardWarningText},{ChokingHazardWarningType}," +
			       $"{Code},{Discontinued},{Hazmat},{string.Join("|", Images)},{IntendedAgeWarningText},{IntendedAgeWarningType},{ManufacturerPartNumber},{MapPrice},{Model},{Msrp},{Name}" +
			       $"{OrderProcess},{Ormd},{string.Join("|", ProductAttributes)},{Prop65Text},{string.Join("|", Recommendations)},{string.Join("|", SeeAlsos)}," +
			       $"{string.Join("|", SmallParts)},{string.Join("|", Substitutes)},{string.Join("|", Supersedes)}," +
			       $"{ThirdPartyAllowed},{Unit},{WeightsAndMeasures}";
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
			return $"<li>{string.Join("<li>", BulletPoints)}|{Code}|{Description}|{Name}";
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

	public class WeightsAndMeasures
	{
		public Measure Height { get; set; }
		public Measure Length { get; set; }
		public Measure Weight { get; set; }
		public Measure Width { get; set; }

		public override string ToString()
		{
			return $"{Height}|{Length}|{Width}|{Weight}";
		}
	}

	public class Measure
	{
		public string Unit { get; set; }
		public double Value { get; set; }

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
