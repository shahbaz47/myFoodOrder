using System;
using System.Collections.Generic;
using System.Collections;
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
    [Activity(Label = "Add Items")]
    public class AddItem : Activity
    {
        EditText edtItemName, edtPrice, edtDesc;
        Button btnAddMenuItem;
        Spinner spinnerCategory, spinnerImg;
        string msg;
        string[] imgDisp = { "Burger", "Pizza", "Burrito", "Fried Chicken", "Rice Bowl", "Sub", "Tacos" };
        string[] imgName = { "burger", "pizza", "burrito", "friedchicken", "ricebowl", "sub", "taco" };
        string[] dropDownList;
        string imgPath;
        Realm myDB;
        int hotelId;
        string myEmail;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            myEmail = Intent.GetStringExtra("email");
            SetContentView(Resource.Layout.AddItem);
            myDB = Realm.GetInstance(config);


            var hotelList = from a in myDB.All<HotelModel>() where (a.hotelName != "") select a;
            List<string> listHotel = new List<string>();
            

            foreach (var u in hotelList)
            {
                listHotel.Add(u.hotelName);
            }

            dropDownList = listHotel.ToArray();

            edtItemName = FindViewById<EditText>(Resource.Id.edtItemName);
            edtPrice = FindViewById<EditText>(Resource.Id.edtPrice);
            edtDesc = FindViewById<EditText>(Resource.Id.edtDesc);
            btnAddMenuItem = FindViewById<Button>(Resource.Id.btnAddMenuItem);


            /* Dropdown code */
            spinnerCategory = FindViewById<Spinner>(Resource.Id.spinnerHotelName);
            ArrayAdapter myDropdownAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, dropDownList);
            spinnerCategory.Adapter = myDropdownAdapter;
            spinnerCategory.ItemSelected += mySpinnerViewIteamSelected;

            spinnerImg = FindViewById<Spinner>(Resource.Id.spinnerImage);
            ArrayAdapter myDropdownAdapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, imgDisp);
            spinnerImg.Adapter = myDropdownAdapter2;
            spinnerImg.ItemSelected += mySpinnerViewIteamSelected2;


            btnAddMenuItem.Click += delegate
            {
                if (edtItemName.Text == "")
                {
                    msg = "Please enter Item Name";
                    Alert(msg);
                }
                else if (edtPrice.Text == "")
                {
                    msg = "Please enter Price";
                    Alert(msg);
                }
                else if (edtDesc.Text == "")
                {
                    msg = "Please enter Description";
                    Alert(msg);
                }
                else
                {
                    ItemModel objItemModel = new ItemModel();

                    int count = myDB.All<ItemModel>().Count();

                    //increatement index
                    int nextID = count + 1;

                    //insert new value
                    objItemModel.id = nextID;
                    objItemModel.hotelId = hotelId;
                    objItemModel.itemName = edtItemName.Text;
                    objItemModel.price = Convert.ToDouble(edtPrice.Text);
                    objItemModel.description = edtDesc.Text;
                    objItemModel.itemImg = imgPath;


                    myDB.Write(() =>
                    {
                        myDB.Add(objItemModel);
                    });
                    Toast.MakeText(this, "Item Added", ToastLength.Short).Show();
                    Intent ItemIntent = new Intent(this, typeof(AddItem));
                    ItemIntent.PutExtra("email", myEmail);
                    StartActivity(ItemIntent);
                }
            };
        }

        private void mySpinnerViewIteamSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var value = dropDownList[e.Position];
            var hotelIdList = from a in myDB.All<HotelModel>() where (a.hotelName == value) select a;

            foreach (var a in hotelIdList)
            {
                hotelId = a.id;
            }
        }
        private void mySpinnerViewIteamSelected2(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            imgPath = imgName[e.Position];
            
        }
        private void Alert(string msg)
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Error");
            alert.SetMessage(msg);
            alert.SetPositiveButton("ok", (senderAiert, args) =>
            {
                
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