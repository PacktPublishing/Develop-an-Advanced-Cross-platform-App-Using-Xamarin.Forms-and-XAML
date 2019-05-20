//
//  INavigationService.cs
//  Navigation Service Interface
//
//  Created by Steven F. Daniel on 20/11/2017.
//  Copyright © 2017 GENIESOFT STUDIOS. All rights reserved.
//
using System;
using System.Threading.Tasks;
using MedicineTracker.ViewModels;
using Xamarin.Forms;

namespace MedicineTracker.Services
{
    public interface INavigationService
    {
        // Asynchronously removes the most recent Page from the navigation stack.
        Task<Page> RemoveViewFromStack();

        // Navigate to a particular ViewModel within our MVVM Model
        Task NavigateTo<TVM>()
            where TVM : BaseViewModel;
    }
}