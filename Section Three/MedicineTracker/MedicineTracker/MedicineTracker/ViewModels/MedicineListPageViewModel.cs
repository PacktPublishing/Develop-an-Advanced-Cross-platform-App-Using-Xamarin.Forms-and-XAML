//
//  MedicineListPageViewModel.cs
//  Medicine List Page View Model Class
//
//  Created by Steven F. Daniel on 22/11/2017.
//  Copyright © 2017 GENIESOFT STUDIOS. All rights reserved.
//
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MedicineTracker.Services;

namespace MedicineTracker.ViewModels
{
    // Declare our MedicineListItem class object
    public class MedicineListItem
    {
        public int Id;
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string SideEffects { get; set; }
        public string DateDoseTaken { get; set; }
    }

    public class MedicineListPageViewModel : BaseViewModel
    {
        // Create our MedicineList Observable Collection
        public ObservableCollection<MedicineListItem> MedicineList;

        // Create and declare our ViewModel class constructor
        public MedicineListPageViewModel(INavigationService navService) : base(navService)
        {
        }

        // Retrieve the Medicine items from our SQLite database
        public void GetMedicineItems()
        {
            // Specify our List Collection to store the items being read
            MedicineList = new ObservableCollection<MedicineListItem>();

            // Iterate through each item stored within our SQLite database
            foreach (var item in new Database.Database().GetItems())
            {
                // Add each item to our MedicineList Collection
                MedicineList.Add(new MedicineListItem
                {
                    Id = item.Id,
                    BrandName = item.BrandName,
                    Description = item.Description,
                    SideEffects = item.SideEffects,
                    DateDoseTaken = item.DateDoseTaken.ToString("dd-MMM-yyyy"),
                });
            }
        }

        // View Model Initialise method
        public override async Task Init()
        {
            await Task.Factory.StartNew(() =>
            {
            });
        }
    }
}