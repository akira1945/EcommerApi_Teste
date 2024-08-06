using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApi.Models
{
    public class Order 
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public int seller_id { get; set; }
        public string delivery_type { get; set; }
        public string order_status { get; set; }
        public decimal total_price { get; set; }
        [NotMapped]
        public string? client_name { get; set; }
        [NotMapped]
        public string? seller_name { get; set; }
        [NotMapped]
        public ICollection<Order_item>? order_items { get; set; }
        [NotMapped]
        public string? client_email { get; set; }
        [NotMapped]
        public string? seller_email { get; set; }

        public Client? client { get; set; }

        public Seller? seller { get; set; }
                
    }
}