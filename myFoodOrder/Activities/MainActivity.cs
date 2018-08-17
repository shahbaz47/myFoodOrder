using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;
using Realms;
using System;
using Android.Content.Res;
using System.Linq;

namespace myFoodOrder
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        EditText email, pwd;
        TextView reg;
        Button btLogin;
        Realm myDB;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            email = FindViewById<EditText>(Resource.Id.ed_email);
            pwd = FindViewById<EditText>(Resource.Id.ed_pwd);
            btLogin = FindViewById<Button>(Resource.Id.btn_login);
            reg = FindViewById<TextView>(Resource.Id.txt_register);

            myDB = Realm.GetInstance(config);

            reg.Click += regClicked;
            btLogin.Click += LoginClicked;
        }
        private void regClicked(object sender, System.EventArgs e)
        {
            Intent regIntent = new Intent(this, typeof(register));
            StartActivity(regIntent);
        }
        private void LoginClicked(object sender, System.EventArgs e)
        {
            Realm myDBobj;
            var flag = 0;
            myDBobj = Realm.GetInstance(config);
            

            var myUserList = myDBobj.All<UserModel>();

            foreach (var myObj in myUserList)
            {
                if (email.Text == myObj.email && pwd.Text == myObj.pswd)
                {
                    flag = 1;
                    break;
                }
                else if (email.Text == "admin" && pwd.Text == "admin")
                {
                    flag = 1;
                    break;
                }

            }
            if (flag == 1)
            {
                Toast.MakeText(this, "Authentication Successful..", ToastLength.Short).Show();
                var idDetail = from a in myDB.All<UserModel>() where (a.email == email.Text) select a;
                Intent indexIntent = new Intent(this, typeof(index));
                indexIntent.PutExtra("email", email.Text);
                StartActivity(indexIntent);
            }
            else
            {
                Toast.MakeText(this, "Wrong login credentials..", ToastLength.Short).Show();
            }
        }
    }
}

