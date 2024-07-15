namespace EcommerceApi.Models
{
    public class Order_item
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public decimal? product_price { get; set; }
        public string? product_title { get; set; }
        

    }
}