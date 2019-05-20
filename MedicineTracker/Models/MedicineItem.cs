//
//  MedicineItem.cs
//  MedicineItem Database Model
//
//  Created by Steven F. Daniel on 21/11/2017.
//  Copyright © 2017 GENIESOFT STUDIOS. All rights reserved.
//
using System;
using SQLite;

namespace MedicineTracker.Models
{
    public class MedicineItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string SideEffects { get; set; }
        public string Dosage { get; set; }
        public DateTime DateDoseTaken { get; set; }
        public TimeSpan TimeDoseTaken { get; set; }
    }
}