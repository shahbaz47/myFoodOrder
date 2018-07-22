using Realms;

public class Items : RealmObject
{
    public string iName { get; set; }
    public string iDesc { get; set; }
    public string iImg { get; set; }
    public string rName { get; set; }
    public double price { get; set; }
}