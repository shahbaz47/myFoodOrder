
using Realms;

public class HotelModel : RealmObject
{
    public int id { get; set; }
    public string hotelName { get; set; }
    public string hotelAddress { get; set; }
    public string hotelImg { get; set; }
}