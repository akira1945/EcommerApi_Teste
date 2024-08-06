namespace EcommerceApi.Models;

public class Shopping_cart_item
{
    public int id { get; set; }
    public int shopping_cart_id { get; set; }
    public int product_id { get; set; }
    public int quantity { get; set; }
    public Product? products { get; set; }
    public Shopping_cart? shopping_Carts { get; set; }
}
