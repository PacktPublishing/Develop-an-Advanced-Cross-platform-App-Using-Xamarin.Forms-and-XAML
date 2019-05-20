//
//  EditMedicineItemPageViewModel.cs
//  Handle Adding or Editing of Medicine Items
//
//  Created by Steven F. Daniel on 22/11/2017.
//  Copyright © 2017 GENIESOFT STUDIOS. All rights reserved.
//
using System;
using System.Threading.Tasks;
using MedicineTracker.Models;
using MedicineTracker.Services;

namespace MedicineTracker.ViewModels
{
    public class EditMedicineItemPageViewModel : BaseViewModel
    {
        // Create and declare our ViewModel class constructor
        public EditMedicineItemPageViewModel(INavigationService navService) : base(navService)
        {
            // If we are creating a new item, we need to update the title
            if (App.SelectedItem == null)
            {
                Title = "Adding Medicine Details";
                App.SelectedItem = new MedicineItem();
                DateDoseTaken = DateTime.Now;
            }
            else
            {
                Title = "Editing Medicine Details";
            }
        }

        // Checks to see if we have entered in a Brand Name and Description
        public bool Save()
        {
            if (App.SelectedItem != null && !string.IsNullOrEmpty(App.SelectedItem.BrandName) && !string.IsNullOrEmpty(App.SelectedItem.Description))
            {
                new Database.Database().SaveItem(App.SelectedItem);
            }
            else
            {
                return false;
            }
            return true;
        }

        // Extract all fields entered within the form
        public string BrandName
        {
            get { return App.SelectedItem.BrandName; }
            set
            {
                App.SelectedItem.BrandName = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return App.SelectedItem.Description; }
            set { App.SelectedItem.Description = value; OnPropertyChanged(); }
        }

        public string SideEffects
        {
            get { return App.SelectedItem.SideEffects; }
            set { App.SelectedItem.SideEffects = value; OnPropertyChanged(); }
        }

        public string Dosage
        {
            get { return App.SelectedItem.Dosage; }
            set { App.SelectedItem.Dosage = value; OnPropertyChanged(); }
        }

        public DateTime DateDoseTaken
        {
            get { return App.SelectedItem.DateDoseTaken; }
            set { App.SelectedItem.DateDoseTaken = value; OnPropertyChanged(); }
        }

        public TimeSpan TimeDoseTaken
        {
            get { return App.SelectedItem.TimeDoseTaken; }
            set { App.SelectedItem.TimeDoseTaken = value; OnPropertyChanged(); }
        }

        public override async Task Init()
        {
            await Task.Factory.StartNew(() =>
            {
                // Check to see if we are creating a new item
                if (App.SelectedItem == null)
                {
                    BrandName = "New";
                    Dosage = "1 per day";
                }
            });
        }
    }
}