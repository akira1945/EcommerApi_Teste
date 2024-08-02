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
}