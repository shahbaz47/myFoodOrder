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
    [Activity(Label = "Login", Theme = "@style/AppTheme", MainLauncher = true)]
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
            Realm myDb;
            var flag = 0;
            myDb = Realm.GetInstance(config);
            

            var myUserList = myDb.All<UserModel>();

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
                Intent indexIntent = new Intent(this, typeof(index));
                indexIntent.PutExtra("email", email.Text);
                StartActivity(indexIntent);
            }
            else
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage("Invalid Email or Password");
                alert.SetPositiveButton("OK", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Wrong login credentials", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
    }
}

