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
    [Activity(Label = "index", MainLauncher = true)]
    public class index : Activity
    {
       // TextView email;
        Realm myDBobj;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        protected override void OnCreate(Bundle savedInstanceState)
        {

           // String user = Intent.GetStringExtra("email");
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            RequestWindowFeature(WindowFeatures.ActionBar);
            //enable navigation mode to support tab layout
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            AddMyTab("Restaurants", new MyRest());
            AddMyTab("My Profile", new MyProfile());


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.index);
            //email = FindViewById<TextView>(Resource.Id.txtUser);

           /* myDBobj = Realm.GetInstance(config);
            var myUserList = from a in myDBobj.All<UserModel>() where (a.email == user) select a;

            foreach (var u in myUserList)
            {
                email.Text += "Welcome " + u.fullName + " !";
            }*/

        }
        void AddMyTab(string tabTitle, Fragment fragment)
        {
            var tab = this.ActionBar.NewTab();

            tab.SetCustomView(Resource.Layout.TabLayout);
            tab.CustomView.FindViewById<TextView>(Resource.Id.myTabTitle).Text = tabTitle;
            

            // must set event handler for replacing tabs
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e) 
            {

                e.FragmentTransaction.Replace(Resource.Id.fragmentContainer, fragment);
            };

            this.ActionBar.AddTab(tab);
        }
    }
}