//
//  MedicineListPage.cs
//  Handle displaying of Medicine Items within the ListView
//
//  Created by Steven F. Daniel on 22/11/2017.
//  Copyright © 2017 GENIESOFT STUDIOS. All rights reserved.
//
using System;
using MedicineTracker.Services;
using MedicineTracker.ViewModels;
using Xamarin.Forms;

namespace MedicineTracker.Pages
{
    public partial class MedicineListPage : ContentPage
    {
        // Return the binding context for our ViewModel
        MedicineListPageViewModel _viewModel
        {
            get { return BindingContext as MedicineListPageViewModel; }
        }

        public MedicineListPage()
        {
            InitializeComponent();

            // Initialise our Page Title
            this.Title = "Medicine Items Listing";

            // Declare and initialise our Model Binding Context
            this.BindingContext = new MedicineListPageViewModel(DependencyService.Get<INavigationService>());
            MedicineListView.ItemSelected += medicineListView_ItemSelected;
        }

        private Action Add()
        {
            return async () =>
            {
                App.SelectedItem = null;
                await _viewModel.Navigation.NavigateTo<EditMedicineItemPageViewModel>();
            };
        }

        async void medicineListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            App.SelectedItem = new Database.Database().GetItem((e.SelectedItem as MedicineListItem).Id);
            await _viewModel.Navigation.NavigateTo<EditMedicineItemPageViewModel>();
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            // Get the selected item to be deleted from our ListView
            var selectedItem = (MedicineListItem)((MenuItem)sender).CommandParameter;

            // Prompt the user with a confirmation dialog to confirm
            var alertResult = await DisplayAlert("Delete Medicine Item", "Proceed and delete medicine item?", "OK", "Cancel");
            if (alertResult == true)
            {
                // Remove item from our SQLite Database and MedicineList collection
                var itemDeleted = new Database.Database().DeleteItem(selectedItem.Id);
                _viewModel.MedicineList.Remove(selectedItem);
            }
            else
                return;
        }

        ToolbarItem toolBarItem;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Initialize our View Model
            if (_viewModel != null)
            {
                await _viewModel.Init();
            }

            // Call our GetMedicineItems method to populate our Collection
            _viewModel.GetMedicineItems();

            // Set up and initialise the binding for our ListView
            MedicineListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, new Binding("."));
            MedicineListView.BindingContext = _viewModel.MedicineList;
            ToolbarItems.Add(toolBarItem = new ToolbarItem("Add", null, Add(), 0, 0));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ToolbarItems.Remove(toolBarItem);
        }
    }
}