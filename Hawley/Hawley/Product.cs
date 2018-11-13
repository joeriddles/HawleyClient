using System.Collections.Generic;
using Newtonsoft.Json;
using static Hawley.Utils;

namespace Hawley
{
	public class ProductAndVariant
	{
		public Product Product;
		public ProductVariant Variant;
	}

	// returned from /Catalog/Products...
	public class Product
	{
		public string ProductId { get; set; }
		public string ProductNo { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string BrandId { get; set; }
		public string Brand { get; set; }
		public string DateCreated { get; set; }
		public string DateLastModified { get; set; }
		public List<string> VariantIds { get; set; }
		public List<string> CategoryIds { get; set; }
		public List<string> CategoryNames { get; set; } = new List<string>();

		public override string ToString()
		{
			return $@"Product ID:	{ProductId}
Product No:	{ProductNo}
Name:	{Name}
Description:	{Description}
Brand ID:	{BrandId}
DateCreated:	{DateCreated}
DateLastModified:	{DateLastModified}
Variants IDs: {Join(VariantIds)}
Category IDs: {Join(CategoryIds)}
";
		}
	}

	// returned from /Catalog/Products/Variants...
	public class ProductVariant
	{
		public string VariantId { get; set; }
		public string VariantNo { get; set; }
		public string ProductId { get; set; }
		public string ProductNo { get; set; }
		public string Description { get; set; }
		public string StatusId { get; set; }
		public string StatusDesc { get; set; }
		[JsonProperty(PropertyName = "UPC")]
		public string Upc { get; set; }
		[JsonProperty(PropertyName = "EAN")]
		public string Ean { get; set; }
		public string Config { get; set; }
		public string Size { get; set; }
		public string Color { get; set; }
		public string ProductDimensionGroup { get; set; }
		public string MfgPartNumber { get; set; }
		[JsonProperty(PropertyName = "MFGPartNumber")]
		public string GroundOnly { get; set; }
		public string DateCreated { get; set; }
		public string DateLastModified { get; set; }
		public List<VariantAttribute> Attributes { get; set; }
		public List<VariantPrice> Prices { get; set; }
		public List<VariantImage> Images { get; set; }
		public List<VariantInventories> Inventories { get; set; } = new List<VariantInventories>();

		public override string ToString()
		{
			return VariantId;
		}

		public class VariantAttribute
		{
			public string VariantId { get; set; }
			public string ProductId { get; set; }
			public List<VariantAttributeInternal> Attributes { get; set; }

			public override string ToString()
			{
				return $@"Variant ID:	{VariantId}
Product ID:	{ProductId}
Attributes:
{Join(Attributes)}
";
			}

			public class VariantAttributeInternal
			{
				public string Name { get; set; }
				public string Value { get; set; }

				public override string ToString()
				{
					return $@"Name: {Name}
Value: {Value}
";
				}
			}
		}

		public class VariantPrice
		{
			public string Price { get; set; }
			public string PriceTypeId { get; set; }
			public string PriceTypeDesc { get; set; }
			public string QtyBreak { get; set; }

			public override string ToString()
			{
				return $@"Price:	{Price}
Price Type ID:	{PriceTypeId},
Price Type Desc:	{PriceTypeDesc}
Qty Break:	{QtyBreak}
";
			}
		}

		public class VariantImage
		{
			public string FormatId { get; set; }
			public string Format { get; set; }
			public string Url { get; set; }

			public override string ToString()
			{
				return $@"Format ID:	{FormatId}
Format:	{Format}
URL: {Url}
";
			}
		}

	}

	public class VariantInventories
	{
		public string VariantId { get; set; }
		public string VariantNo { get; set; }
		public string ProductNo { get; set; }
		public bool AddedToWatchList { get; set; }
		public string StatusId { get; set; }
		public string StatusDescription { get; set; }
		public List<WarehouseQuantity> WarehousesQuantities { get; set; }
		public string TotalQuantityAvailable { get; set; }

		public override string ToString()
		{
			return $@"Variant ID:	{VariantId}
Variant No:	{VariantNo}
Product No:	{ProductNo}
Added To Watch List:	{AddedToWatchList}
Status ID:	{StatusId}
Status Description:	{StatusDescription}
Warehouse Quantity:	{Join(WarehousesQuantities)}
Total Quantity Available:	{TotalQuantityAvailable}
";
		}

		public class WarehouseQuantity
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string QtyAvailable { get; set; }

			public override string ToString()
			{
				return $@"ID:	{Id}
Name:	{Name}
Qty Available:	{QtyAvailable}
";
			}
		}
	}

	public class Categories
	{
		public string CategoryId { get; set; }
		public string CategoryName { get; set; }
		public List<SubCategory> SubCategories { get; set; }

		public class SubCategory
		{
			public string CategoryId { get; set; }
			public string CategoryName { get; set; }
			public List<SubCategory> SubCategories { get; set; }
		}
	}
}
