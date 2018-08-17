
using Realms;

public class CartModel : RealmObject
{
    public int id { get; set; }
    public string userId { get; set; }
    public int itemId { get; set; }
    public int hotelId { get; set; }
    public int qty { get; set; }
}
