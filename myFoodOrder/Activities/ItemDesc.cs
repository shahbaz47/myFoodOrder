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
    [Activity(Label = "ItemDesc")]
    public class ItemDesc : Activity
    {
        ImageView imgItem;
        TextView txtItemName, txtPrice, txtDesc;
        EditText edtQty;
        Button btnAddCart;
        Realm myDB;
        string msg;
        String myEmail;
        int hotelId;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion=1 };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            myEmail = Intent.GetStringExtra("email");
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.ItemDesc);
            myDB = Realm.GetInstance(config);

            imgItem = FindViewById<ImageView>(Resource.Id.imgItem);
            txtItemName = FindViewById<TextView>(Resource.Id.txtItemName);
            txtPrice = FindViewById<TextView>(Resource.Id.txtPrice);
            txtDesc = FindViewById<TextView>(Resource.Id.txtDesc);
            edtQty = FindViewById<EditText>(Resource.Id.edtQty);
            btnAddCart = FindViewById<Button>(Resource.Id.btnAddToCart);
            if (myEmail == "admin")
            {
                btnAddCart.Visibility = ViewStates.Invisible;
                edtQty.Visibility = ViewStates.Invisible;
            }
            int itemId = Convert.ToInt32(Intent.GetStringExtra("itemId"));
            var itemDetail = from a in myDB.All<ItemModel>() where (a.id == itemId) select a;

            int imgRes;
            
            
            foreach (var item in itemDetail)
            {
                itemId = item.id;
                hotelId = item.hotelId;
                txtItemName.Text = item.itemName;
                txtDesc.Text = item.description;
                txtPrice.Text = "$" + item.price.ToString();
                imgRes = (int)typeof(Resource.Drawable).GetField(item.itemImg).GetValue(null);
                imgItem.SetImageResource(imgRes);
            }
            

            btnAddCart.Click += delegate
            {

                if (edtQty.Text == "")
                {
                    msg = "Please enter Description";
                    Alert(msg);
                }
                else
                {
                    CartModel objCartModel = new CartModel();

                    int count = myDB.All<CartModel>().Count();

                    //increatement index
                    int nextID = count + 1;

                    //insert new value
                    objCartModel.id = nextID;
                    objCartModel.itemId = itemId;
                    objCartModel.qty = Convert.ToInt32(edtQty.Text);
                    objCartModel.hotelId = hotelId;
                    objCartModel.userId = myEmail;


                    myDB.Write(() =>
                    {
                        myDB.Add(objCartModel);
                    });
                    Toast.MakeText(this, "" + "Added to Cart Successfully", ToastLength.Short).Show();
                }
            };
        }
        private void Alert(string msg)
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Error");
            alert.SetMessage(msg);
            alert.SetPositiveButton("ok", (senderAiert, args) =>
            {
                //Toast.MakeText(this, "Empty", ToastLength.Long).Show();
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