namespace EcommerceApi.ProductsDTOs
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

}