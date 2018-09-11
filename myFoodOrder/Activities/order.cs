using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace myFoodOrder
{
    [Activity(Label = "Order Successful")]
    public class order : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TextView goHome;
            string myEmail;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.order);
            goHome = FindViewById<TextView>(Resource.Id.txtLink);
            myEmail = Intent.GetStringExtra("email");

            goHome.Click += delegate
            {
                Intent i = new Intent(this, typeof(index));
                i.PutExtra("email", myEmail);
                StartActivity(i);
            };
        }
    }
}