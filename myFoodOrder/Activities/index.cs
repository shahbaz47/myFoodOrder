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
using Android.Text;

namespace myFoodOrder
{
    [Activity(Label = "My Food Order")]
    public class index : Activity
    {
        Realm myDb;
        string fullName;
        string myEmail;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        protected override void OnCreate(Bundle savedInstanceState)
        {       

            myEmail = Intent.GetStringExtra("email");
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.ActionBar);

            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            AddMyTab("Restaurants", new MyRest(this, myEmail));
            AddMyTab("My Profile", new MyProfile(this, myEmail));
            SetContentView(Resource.Layout.index);
            myDb = Realm.GetInstance(config);
            var myUserList = from a in myDb.All<UserModel>() where (a.email == myEmail) select a;
            foreach (var u in myUserList)
            {
                fullName = u.fullName;
            }
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            if (myEmail != "admin")
            {
                if(fullName.Contains(" "))
                    fullName = fullName.Split(' ')[0];
                Window.SetTitle("Welcome " + fullName);
            }
            else
            {
                Window.SetTitle("Welcome Administrator");
            }
                
        }
        void AddMyTab(string tabTitle, Fragment fragment)
        {
            var tab = this.ActionBar.NewTab();

            tab.SetCustomView(Resource.Layout.TabLayout);
            tab.CustomView.FindViewById<TextView>(Resource.Id.myTabTitle).Text = tabTitle;
            
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e) 
            {
                e.FragmentTransaction.Replace(Resource.Id.fragmentContainer, fragment);
            };

            this.ActionBar.AddTab(tab);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // set the menu layout on Main Activity  
            MenuInflater.Inflate(Resource.Menu.mainMenu, menu);
            IMenuItem myCrt = menu.FindItem(Resource.Id.menuItem2);
            IMenuItem addRest = menu.FindItem(Resource.Id.menuItem3);
            IMenuItem addItem = menu.FindItem(Resource.Id.menuItem4);
            if (myEmail == "admin")
            {
                addRest.SetVisible(true);
                addItem.SetVisible(true);
                myCrt.SetVisible(false);
            }
            else
            {
                addRest.SetVisible(false);
                addItem.SetVisible(false);
                myCrt.SetVisible(true);
            }
            
            
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuItem1:
                    {
                        Intent i = new Intent(this, typeof(index));
                        i.PutExtra("email", myEmail);
                        StartActivity(i);
                        return true;
                    }
                case Resource.Id.menuItem2:
                    {
                        Intent CartIntent = new Intent(this, typeof(CartView));
                        CartIntent.PutExtra("email", myEmail);
                        StartActivity(CartIntent);
                        return true;
                    }
                case Resource.Id.menuItem3:
                    {
                        Intent restIntent = new Intent(this, typeof(AddRestaurant));
                        restIntent.PutExtra("email", myEmail);
                        StartActivity(restIntent);
                        return true;
                    }
                case Resource.Id.menuItem4:
                    {
                        Intent ItemIntent = new Intent(this, typeof(AddItem));
                        ItemIntent.PutExtra("email", myEmail);
                        StartActivity(ItemIntent);
                        return true;
                    }
                case Resource.Id.menuItem5:
                    {
                        Toast.MakeText(this, "Successfully Logged Out..", ToastLength.Short).Show();
                        Intent mainIntent = new Intent(this, typeof(MainActivity));
                        StartActivity(mainIntent);
                        return true;
                    }
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}