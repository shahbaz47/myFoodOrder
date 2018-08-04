using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realms;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace myFoodOrder
{
    [Activity(Label = "index")]
    public class index : Activity
    {
        TextView email;
        Realm myDBobj;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            String user = Intent.GetStringExtra("email");
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.index);

            email = FindViewById<TextView>(Resource.Id.txtUser);

            myDBobj = Realm.GetInstance(config);
            var myUserList = from a in myDBobj.All<UserModel>() where (a.email == user) select a;

            foreach (var u in myUserList)
            {
                email.Text += "Welcome " + u.fullName + " !";
            }

        }
    }
}