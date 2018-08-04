using Realms;

public class UserModel : RealmObject
{
    
    [PrimaryKey] public string email { get; set; }
    public string fullName { get; set; }
    public string pswd { get; set; }
    public string phNo { get; set; }
    public string age { get; set; }

}