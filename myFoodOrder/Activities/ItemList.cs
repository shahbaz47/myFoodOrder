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

namespace myFoodOrder
{
    class ItemList : BaseAdapter<ItemModel>
    {
        List<ItemModel> myArrayList;
        Activity context;

        public ItemList(Activity contextInfo, List<ItemModel> userlist)
        {
            context = contextInfo;
            myArrayList = userlist;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override ItemModel this[int position]
        {
            get { return myArrayList[position]; }
        }
        public override int Count
        {
            get { return myArrayList.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            ItemModel objItemModel = myArrayList[position];

            View myView = convertView;
            if (myView == null)
            {
                myView = context.LayoutInflater.Inflate(Resource.Layout.ItemList, null);
            }

            int imgRes = (int)typeof(Resource.Drawable).GetField(objItemModel.itemImg).GetValue(null);
            myView.FindViewById<TextView>(Resource.Id.nameId).Text = objItemModel.itemName;
            myView.FindViewById<TextView>(Resource.Id.txtPrice).Text = objItemModel.price.ToString();
            myView.FindViewById<ImageView>(Resource.Id.imgId).SetImageResource(imgRes);

            return myView;
        }
    }
}