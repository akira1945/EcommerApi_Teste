namespace EcommerceApi.Models
{
    public class Product
    {
        public int id { get; set; }
        public required string title { get; set; }
        public required string  description { get; set; }
        public string? image_url { get; set; }
        public float price { get; set; }
        public int stock { get; set; }

    }
}