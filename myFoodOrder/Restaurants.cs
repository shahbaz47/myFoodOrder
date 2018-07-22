using Realms;

public class Restaurants : RealmObject
{
    public string rName { get; set; }
    public string rAddr { get; set; }
    public string rImg { get; set; }
    public string rType { get; set; }
}