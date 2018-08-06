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
        EditText email, pwd, fName, phNo, age;
        Button btReg;
        Realm myDB;
        RealmConfiguration config = new RealmConfiguration() { SchemaVersion = 1 };
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            email = FindViewById<EditText>(Resource.Id.ed_email);
            pwd = FindViewById<EditText>(Resource.Id.ed_pwd);
            fName = FindViewById<EditText>(Resource.Id.ed_fName);
            phNo = FindViewById<EditText>(Resource.Id.ed_phNo);
            age = FindViewById<EditText>(Resource.Id.ed_age);
            btReg = FindViewById<Button>(Resource.Id.btn_reg);

            myDB = Realm.GetInstance(config);

            btReg.Click += regClicked;
        }
        private void regClicked(object sender, System.EventArgs e)
        {
            var nameValue = fName.Text;
            var emailValue = email.Text;
            var ageValue = age.Text;
            var phoneValue = phNo.Text;
            var pwdValue = pwd.Text;

            if (emailValue != "" && pwdValue != "" && nameValue != "" && ageValue != "" && phoneValue != "")
            {
                Toast.MakeText(this, "Successfully Registered..", ToastLength.Short).Show();

                var myUserModel = new UserModel();
                myUserModel.fullName = nameValue;
                myUserModel.pswd = pwdValue;
                myUserModel.email = emailValue;
                myUserModel.age = ageValue;
                myUserModel.phNo = phoneValue;
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