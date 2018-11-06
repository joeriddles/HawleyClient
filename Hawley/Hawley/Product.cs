using System.Collections.Generic;

namespace Hawley
{
	// returned from /Catalog/Products...
	class Product
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
	}

	// returned from /Catalog/Products/Variants...
	class ProductVariant
	{
		public string VariantId { get; set; }
		public string VariantNo { get; set; }
		public string ProductId { get; set; }
		public string ProductNo { get; set; }
		public string Description { get; set; }
		public string StatusId { get; set; }
		public string StatusDesc { get; set; }
		public string Upc { get; set; }
		public string Ean { get; set; }
		public string Config { get; set; }
		public string Size { get; set; }
		public string Color { get; set; }
		public string ProductDimensionGroup { get; set; }
		public string MfgPartNumber { get; set; }
		public bool GroundOnly { get; set; }
		public string DateCreated { get; set; }
		public string DateLastModified { get; set; }
		public List<VariantAttribute> Attributes { get; set; }
		public List<VariantPrice> Prices { get; set; }
		public List<VariantImage> Images { get; set; }

		public class VariantAttribute
		{
			public string VariantId { get; set; }
			public string ProductId { get; set; }
			public List<VariantAttributeInternal> Attributes { get; set; }

			public class VariantAttributeInternal
			{
				public string Name { get; set; }
				public string Value { get; set; }
			}
		}

		public class VariantPrice
		{
			public string Price { get; set; }
			public string PriceTypeId { get; set; }
			public string PriceTypeDesc { get; set; }
			public string QtyBreak { get; set; }
		}

		public class VariantImage
		{
			public string FormatId { get; set; }
			public string Format { get; set; }
			public string Url { get; set; }
		}
	}
}
