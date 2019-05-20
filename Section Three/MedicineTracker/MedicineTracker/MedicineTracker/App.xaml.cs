//
//  App.xaml.cs
//  Main Application Class
//
//  Created by Steven F. Daniel on 21/11/2017.
//  Copyright © 2017 GENIESOFT STUDIOS. All rights reserved.
//
using MedicineTracker.Models;
using MedicineTracker.Pages;
using MedicineTracker.Services;
using MedicineTracker.ViewModels;
using Xamarin.Forms;

namespace MedicineTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // The root page of our application
            var mainPage = new NavigationPage(new MedicineListPage());

            // Create an instance of our NavigationService service
            var navService = DependencyService.Get<INavigationService>() as NavigationService;

            // Assign the main page to our navigation service
            navService.XFNavigation = mainPage.Navigation;

            // Register each of our View Models on our Navigation Stack
            navService.RegisterViewMapping(typeof(MedicineListPageViewModel), typeof(MedicineListPage));
            navService.RegisterViewMapping(typeof(EditMedicineItemPageViewModel), typeof(EditMedicineItemPage));

            // Set the root page of your application
            MainPage = mainPage;
        }

        // Declare our Medicine Item model that we will use to store
        // our temporary medicine details
        public static MedicineItem SelectedItem { get; set; }

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