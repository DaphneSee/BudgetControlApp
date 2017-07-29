using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BudgetControl
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new BudgetControl.MainPage();
            MainPage = new TabbedPage
            {
                Children = {
                    new MainPage(),
                    //new TabbedPage1(),
                    new AzureTable()

                }
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
