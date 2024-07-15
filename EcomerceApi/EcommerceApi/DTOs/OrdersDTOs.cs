namespace EcommerceApi.DTOs
{
    public class CreateOrderDto
    {
        public int client_id { get; set; }
        public int seller_id { get; set; }
        public string delivery_type { get; set; }
        public IEnumerable<ProductToCreateOrderDto> products { get; set; }

    }
}