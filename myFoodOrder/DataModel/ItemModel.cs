using Realms;

public class ItemModel : RealmObject
{
    public int id { get; set; }
    public int hotelId { get; set; }
    public string itemName { get; set; }
    public double price { get; set; }
    public string description { get; set; }
    public string itemImg { get; set; }
}
