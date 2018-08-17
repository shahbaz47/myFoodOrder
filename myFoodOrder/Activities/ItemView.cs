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
using myFoodOrder;
using Realms;



namespace myFoodOrder
{
    [Activity(Label = "ItemView")]
    public class ItemView : Activity
    {
        ListView myListView;
        SearchView mySearchBar;
        Realm myDb;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        List<ItemModel> myList = new List<ItemModel>();
        String myEmail;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            myEmail = Intent.GetStringExtra("email");
            base.OnCreate(savedInstanceState);
            int hId = Convert.ToInt32(Intent.GetStringExtra("hotelId"));
            // Create your application here
            SetContentView(Resource.Layout.ItemView);
            myDb = Realm.GetInstance(config);

            myListView = FindViewById<ListView>(Resource.Id.itemListId);
            mySearchBar = FindViewById<SearchView>(Resource.Id.searchId);

            //var myListAll = myDb.All<ItemModel>();
            var myListAll = from a in myDb.All<ItemModel>() where (a.hotelId == hId) select a;

            foreach (var myObj in myListAll)
            {
                myList.Add(myObj);
            }

            ItemList myOwnAdapter = new ItemList(this, myList);

            myListView.Adapter = myOwnAdapter;
            myListView.ItemClick += myListViewClick;
            mySearchBar.QueryTextChange += mySearchBarMethod;

        }
        public void myListViewClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var value = myList[e.Position];
            Intent itIntent = new Intent(this, typeof(ItemDesc));
            itIntent.PutExtra("itemId", value.id.ToString());
            itIntent.PutExtra("email", myEmail);
            StartActivity(itIntent);
        }
        public void mySearchBarMethod(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            var searchedText = e.NewText;
            System.Console.WriteLine("SearchText \n" + searchedText);


            var searchedArray = myFilterMethod(searchedText);

            ItemList adapter = new ItemList(this, searchedArray);
            myListView.Adapter = adapter;
        }


        public List<ItemModel> myFilterMethod(string searchedText)
        {
            var filterArray = new List<ItemModel>();

            foreach (var itemModel in myList)
            {
                var item = itemModel.itemName;
                item = item.ToLower();

                if (item.Contains(searchedText.ToLower()))
                {

                    filterArray.Add(itemModel);
                }
            }

            return filterArray;
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