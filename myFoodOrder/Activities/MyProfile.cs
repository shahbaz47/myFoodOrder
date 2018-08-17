using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Realms;

namespace myFoodOrder
{
    public class MyProfile : Fragment
    {
        String myEmail;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        private Activity myContext;

        public MyProfile(Activity context, string emailId)
        {
           myContext = context;
            myEmail = emailId;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           
            View view = inflater.Inflate(Resource.Layout.MyProfile, container, false);

            EditText txtFname, txtEmail, txtPhone, txtAge;
            Button update;
            Realm myDB;
            
            txtFname = view.FindViewById<EditText>(Resource.Id.txtFname);
            txtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            txtPhone = view.FindViewById<EditText>(Resource.Id.txtPhone);
            txtAge = view.FindViewById<EditText>(Resource.Id.txtAge);
            update = view.FindViewById<Button>(Resource.Id.btnUpdate);

            myDB = Realm.GetInstance(config);
            if (myEmail != "admin")
            {
                var myUserList = (from a in myDB.All<UserModel>() where (a.email == myEmail) select a).First();

                txtEmail.Text = myUserList.email;
                txtFname.Text = myUserList.fullName;
                txtPhone.Text = myUserList.phNo;
                txtAge.Text = myUserList.age;


                update.Click += delegate
                 {

                     if (txtEmail.Text != "" && txtAge.Text != "" && txtFname.Text != "" && txtPhone.Text != "")
                     {
                         myDB.Write(() =>
                         {
                             myUserList.fullName = txtFname.Text;
                             myUserList.email = txtEmail.Text;
                             myUserList.age = txtAge.Text;
                             myUserList.phNo = txtPhone.Text;
                         });
                         Toast.MakeText(this.Context, "SuccessFully Updated !", ToastLength.Short).Show();
                     }
                     else
                     {
                         Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this.Context);
                         alert.SetTitle("Error");
                         alert.SetMessage("Please Enter all fields..");
                         alert.SetPositiveButton("OK", (senderAlert, args) =>
                         {
                             Toast.MakeText(this.Context, "Insuffecient Data !", ToastLength.Short).Show();
                         });
                         Dialog dialog = alert.Create();
                         dialog.Show();
                     }
                 };
            }
            else
            {
                txtEmail.Text = "admin@mfo.com";
                txtEmail.Enabled = false;
                txtFname.Text = "Administrator";
                txtFname.Enabled = false;
                txtPhone.Text = "6479876541";
                txtPhone.Enabled = false;
                txtAge.Text = "27";
                txtAge.Enabled = false;
                update.Visibility = ViewStates.Invisible;
            }
            
                return view;
        }
        
    }
}