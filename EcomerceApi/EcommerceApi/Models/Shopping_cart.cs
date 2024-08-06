namespace EcommerceApi.Models;

public class Shopping_cart
{
    public int id { get; set; }
    public int client_id { get; set; }
    public decimal total_price { get; set; }
    public Client? client { get; set; }
    public IEnumerable<Shopping_cart_item>? shopping_Cart_Items { get; set; }

}
