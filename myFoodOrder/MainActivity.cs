using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;
using Realms;
using System;
using Android.Content.Res;

namespace myFoodOrder
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        EditText userName, pwd;
        TextView reg;
        Button btLogin;
        Realm myDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            userName = FindViewById<EditText>(Resource.Id.ed_usr);
            pwd = FindViewById<EditText>(Resource.Id.ed_pwd);
            btLogin = FindViewById<Button>(Resource.Id.btn_login);
            reg = FindViewById<TextView>(Resource.Id.txt_register);

            myDB = Realm.GetInstance();

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
            myDBobj = Realm.GetInstance();
            var myUserList = myDBobj.All<UserModel>();

            foreach (var myObj in myUserList)
            {
                if (userName.Text == myObj.userName && pwd.Text == myObj.pswd)
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                Toast.MakeText(this, "Authentication Successful..", ToastLength.Short).Show();

                Intent indexIntent = new Intent(this, typeof(index));
                indexIntent.PutExtra("userName", userName.Text);
                StartActivity(indexIntent);
            }
            else
            {
                Toast.MakeText(this, "Wrong login credentials..", ToastLength.Short).Show();
            }
        }
    }
}

