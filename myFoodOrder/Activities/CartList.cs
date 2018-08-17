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

    class CartList : BaseAdapter<CartModel>
    {
        Realm myDB;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        string itemNm;
        double itemPrice;
        int imgRes;

        List<CartModel> myArrayList;
        Activity context;

        public CartList(Activity contextInfo, List<CartModel> userlist)
        {
            context = contextInfo;
            myArrayList = userlist;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override CartModel this[int position]
        {
            get { return myArrayList[position]; }
        }
        public override int Count
        {
            get { return myArrayList.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            myDB = Realm.GetInstance(config);
            CartModel objCartModel = myArrayList[position];
            View myView = convertView;
            if (myView == null)
            {
                myView = context.LayoutInflater.Inflate(Resource.Layout.CartList, null);
            }
            var itemDetail = from a in myDB.All<ItemModel>() where (a.id == objCartModel.itemId) select a;
            foreach (var d in itemDetail)
            {
                itemNm = d.itemName;
                itemPrice = d.price;
                imgRes = (int)typeof(Resource.Drawable).GetField(d.itemImg).GetValue(null);
            }
            
            myView.FindViewById<TextView>(Resource.Id.nameId).Text = itemNm;
            myView.FindViewById<TextView>(Resource.Id.txtQty).Text = "Quantity : " + objCartModel.qty.ToString();
            myView.FindViewById<TextView>(Resource.Id.txtPrice).Text = "$" + (itemPrice * objCartModel.qty).ToString();
            myView.FindViewById<ImageView>(Resource.Id.imgId).SetImageResource(imgRes);

            return myView;
        }
    }
}