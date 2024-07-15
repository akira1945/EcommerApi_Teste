namespace EcommerceApi.DTOs
{
    public class CreateProductDto
    {
        public required string title { get; set; }
        public required string  description { get; set; }
        public string? image_url { get; set; }
        public float price { get; set; }
        public int stock { get; set; }
    }

    public class ListProductStockDto
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string?  description { get; set; }
        public int? stock { get; set; }
    }

    public class UpdateProductDto
    {
        public string? title { get; set; }
        public string?  description { get; set; }
        public string? image_url { get; set; }
    }
    public class UpdatePriceProductDto
    {
        public float? price { get; set; }
        
    }

    public class UpdateStockProductDto
    {
        public int? stock { get; set; }
    }

     public class DeleteProductDto
    {
        public int? id { get; set; }
        public string? title { get; set; }
        public string?  description { get; set; }
    }

     public class DeleteSqlDto
    {
        public int id { get; set; }
        
    }

    public class ListProductSqlDto
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string?  description { get; set; }
        public int? stock { get; set; }
    }

    public class UpdateProductSqlDto
    {
        public int id { get; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? image_url { get; set; }
        public float? price { get; set; }
        public int? stock { get; set; }
    }

    public class ProductToCreateOrderDto
    {
        public int id { get; set; }
        public int quantity { get; set; }
    }

}