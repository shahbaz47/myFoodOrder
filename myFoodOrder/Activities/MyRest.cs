using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using myFoodOrder;
using Realms;

namespace myFoodOrder
{
    public class MyRest : Fragment
    {
        ListView myListView;
        SearchView mySearchBar;
        Realm myDb;List<HotelModel> myList = new List<HotelModel>();
        String myEmail;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        private Activity myContext;

        public MyRest(Activity context, string emailId)
        {
            myContext = context;
            myEmail = emailId;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.MyRest, container, false);
           
            myList.Clear();
            

            myDb = Realm.GetInstance(config);

            myListView = view.FindViewById<ListView>(Resource.Id.restListId);
            mySearchBar = view.FindViewById<SearchView>(Resource.Id.searchId);

            var myListAll = myDb.All<HotelModel>();

            foreach (var myObj in myListAll)
            {
                myList.Add(myObj);
            }

            RestaurantList myOwnAdapter = new RestaurantList(this.Activity, myList);
            myListView.Adapter = myOwnAdapter;
            mySearchBar.QueryTextChange += mySearchBarMethod;

            myListView.ItemClick += myListViewClick;

            return view;
        }
        public void mySearchBarMethod(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            var searchedText = e.NewText;
            var searchedArray = myFilterMethod(searchedText);

            RestaurantList adapter = new RestaurantList(this.Activity, searchedArray);
            myListView.Adapter = adapter;
        }
        public void myListViewClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var value = myList[e.Position];
            Intent itIntent = new Intent(this.Context, typeof(ItemView));
            itIntent.PutExtra("hotelId", value.id.ToString());
            itIntent.PutExtra("email", myEmail);
            Toast.MakeText(this.Context, "Hotel ID" + value.id.ToString(), ToastLength.Long).Show();
            StartActivity(itIntent);
        }


        public List<HotelModel> myFilterMethod(string searchedText)
        {
            var filterArray = new List<HotelModel>();

            foreach (var hotelModel in myList)
            {
                var hotel = hotelModel.hotelName;
                hotel = hotel.ToLower();

                if (hotel.Contains(searchedText.ToLower()))
                {

                    filterArray.Add(hotelModel);
                }
            }

            return filterArray;
        }
    }
}