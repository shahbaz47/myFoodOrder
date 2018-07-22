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
        TextView userName;
        Realm myDBobj;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            String user = Intent.GetStringExtra("userName");
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.index);

            userName = FindViewById<TextView>(Resource.Id.txtUser);

            myDBobj = Realm.GetInstance();
            var myUserList = from a in myDBobj.All<UserModel>() where (a.userName == user) select a;

            foreach (var u in myUserList)
            {
                userName.Text += u.email;
            }

        }
    }
}