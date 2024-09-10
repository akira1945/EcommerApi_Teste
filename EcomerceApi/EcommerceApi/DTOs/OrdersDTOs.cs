using EcommerceApi.Data;
using EcommerceApi.Repositories;

namespace EcommerceApi.DTOs
{
    public class CreateOrderDto
    {
        public int client_id { get; set; }
        public int seller_id { get; set; }
        public string delivery_type { get; set; }
        public IEnumerable<ProductToCreateOrderDto> products { get; set; }


    }

    public class ListOrdersIdDto
    {
        public int id { get; set; }
    }

    public class InsertedOrderDTO
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public int seller_id { get; set; }
        public string delivery_type { get; set; }
        public string order_status { get; set; }
        public decimal total_price { get; set; }
        public string? client_name { get; set; }
        public string? seller_name { get; set; }
        public string? client_email { get; set; }
        public string? seller_email { get; set; }
        
    }
}