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
    [Activity(Label = "register")]
    public class register : Activity
    {
        EditText uName, email, pwd;
        Button btReg;
        Realm myDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            email = FindViewById<EditText>(Resource.Id.ed_email);
            pwd = FindViewById<EditText>(Resource.Id.ed_pwd);
            uName = FindViewById<EditText>(Resource.Id.ed_uName);
            btReg = FindViewById<Button>(Resource.Id.btn_reg);
            
            myDB = Realm.GetInstance();

            btReg.Click += regClicked;
        }
        private void regClicked(object sender, System.EventArgs e)
        {
            var unameValue = uName.Text;
            var emailValue = email.Text;
            var pwdValue = pwd.Text;

            if (emailValue != "" && pwdValue != "" && unameValue != "")
            {
                Toast.MakeText(this, "Successfully Registered..", ToastLength.Short).Show();

                var myUserModel = new UserModel();
                myUserModel.userName = unameValue;
                myUserModel.pswd = pwdValue;
                myUserModel.email = emailValue;
                myDB.Write(() =>
                {
                    myDB.Add(myUserModel);
                });
                Intent mainIntent = new Intent(this, typeof(MainActivity));
                StartActivity(mainIntent);
            }
            else
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage("Please Enter all fields..");
                alert.SetPositiveButton("OK", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Insuffecient data..", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
    }
}