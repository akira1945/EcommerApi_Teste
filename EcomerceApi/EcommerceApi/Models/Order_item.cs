using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApi.Models
{
    public class Order_item
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        [NotMapped]
        public decimal? product_price { get; set; }
        [NotMapped]
        public string? product_title { get; set; }
        public Order? order { get; set; }
        public Product? product { get; set; }        

    }
}