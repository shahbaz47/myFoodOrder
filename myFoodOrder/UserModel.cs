using Realms;

public class UserModel : RealmObject
{
    public string userName { get; set; }
    public string email { get; set; }
    public string pswd { get; set; }
}