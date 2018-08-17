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
using Realms;

namespace myFoodOrder
{
    [Activity(Label = "CartView")]
    public class CartView : Activity
    {
        ListView myListView;
        Realm myDb;
        double itemPrice;
        double totalPrice;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        List<CartModel> myList = new List<CartModel>();
        String myEmail;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            myEmail = Intent.GetStringExtra("email");
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CartView);
            myDb = Realm.GetInstance(config);

            myListView = FindViewById<ListView>(Resource.Id.cartListId);
            var myListAll = from a in myDb.All<CartModel>() where (a.userId == myEmail) select a;

            foreach (var myObj in myListAll)
            {
                myList.Add(myObj);
                var itemDetail = from a in myDb.All<ItemModel>() where (a.id == myObj.itemId) select a;
                foreach (var d in itemDetail)
                {
                    itemPrice = d.price;
                }
                totalPrice = totalPrice + (itemPrice * myObj.qty);                
            }

            CartList myOwnAdapter = new CartList(this, myList);
            myListView.ItemClick += myListViewClick;
            myListView.Adapter = myOwnAdapter;
            FindViewById<TextView>(Resource.Id.txtTotal).Text += totalPrice.ToString();

        }
        public void myListViewClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var value = myList[e.Position];
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Delete");
            alert.SetMessage("This will delete selected item from you Cart. Do you want to delete it?");
            alert.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                var del = myDb.All<CartModel>().First(b => b.id == value.id);
                using (var trans = myDb.BeginWrite())
                {
                    myDb.Remove(del);
                    trans.Commit();
                    Toast.MakeText(this, "Delete Successful..", ToastLength.Short).Show();
                }
                Intent CartIntent = new Intent(this, typeof(CartView));
                CartIntent.PutExtra("email", myEmail);
                StartActivity(CartIntent);
            });
            Dialog dialog = alert.Create();
            dialog.Show();


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