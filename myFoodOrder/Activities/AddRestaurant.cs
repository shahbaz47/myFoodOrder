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
    [Activity(Label = "Add Restaurants")]
    public class AddRestaurant : Activity
    {
        EditText edtHotelName, edtAddress;
        Button btnAddHotel;
        string msg, myEmail, imgPath;
        string[] imgDisp = { "McDonalds", "KFC", "Taco Bell", "Chipotle", "Pizza Pizza", "Subway" };
        string[] imgName = { "mcd", "kfc", "tacobell", "chipotle", "pizzapizza", "subway" };
        Spinner spinnerImg;
        Realm myDB;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.AddRestaurant);
            myDB = Realm.GetInstance(config);
            myEmail = Intent.GetStringExtra("email");

            edtHotelName = FindViewById<EditText>(Resource.Id.edtHotelName);
            edtAddress = FindViewById<EditText>(Resource.Id.edtAddress);
            btnAddHotel = FindViewById<Button>(Resource.Id.btnAddHotel);
          

            spinnerImg = FindViewById<Spinner>(Resource.Id.spinnerImage);
            ArrayAdapter myDropdownAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, imgDisp);
            spinnerImg.Adapter = myDropdownAdapter;
            spinnerImg.ItemSelected += mySpinnerViewIteamSelected;

            btnAddHotel.Click += delegate
            {
                if (edtHotelName.Text == "")
                {
                    msg = "Please enter Hotel Name";
                    Alert(msg);
                }
                else if (edtAddress.Text == "")
                {
                    msg = "Please enter valid Age";
                    Alert(msg);
                }
                else
                {
                    HotelModel objHotelModel = new HotelModel();
                    int count = myDB.All<HotelModel>().Count();
                    //increatement index
                    int nextID = count + 1;
                    //insert new value
                    objHotelModel.id = nextID;
                    objHotelModel.hotelName = edtHotelName.Text;
                    objHotelModel.hotelAddress = edtAddress.Text;
                    objHotelModel.hotelImg = imgPath;


                    myDB.Write(() =>
                    {
                        myDB.Add(objHotelModel);
                    });
                    Toast.MakeText(this, "Hotel Added !", ToastLength.Short).Show();
                    Intent restIntent = new Intent(this, typeof(AddRestaurant));
                    restIntent.PutExtra("email", myEmail);
                    StartActivity(restIntent);
                }


            };

        }
        private void mySpinnerViewIteamSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            imgPath= imgName[e.Position];
            Toast.MakeText(this, imgPath, ToastLength.Long).Show();
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