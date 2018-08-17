using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace myFoodOrder
{
    class RestaurantList : BaseAdapter<HotelModel>
    {
        List<HotelModel> myArrayList;
        Activity context;

        public RestaurantList(Activity contextInfo, List<HotelModel> userlist)
        {
            context = contextInfo;
            myArrayList = userlist;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override HotelModel this[int position]
        {
            get { return myArrayList[position]; }
        }

        public override int Count
        {
            get { return myArrayList.Count; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            HotelModel hm = myArrayList[position];

            View myView = convertView;
            //View myView = inflater.Inflate(Resource.Layout.MyRest, container, false);
            if (myView == null)
            {
                myView = context.LayoutInflater.Inflate(Resource.Layout.RestaurantList,null);
            }
            int imgRes = (int)typeof(Resource.Drawable).GetField(hm.hotelImg).GetValue(null);
            myView.FindViewById<TextView>(Resource.Id.restNameId).Text = hm.hotelName;
            myView.FindViewById<TextView>(Resource.Id.restAddr).Text = hm.hotelAddress;
            myView.FindViewById<ImageView>(Resource.Id.resImgId).SetImageResource(imgRes);

            return myView;
        }
    }
}