//
//  Database.cs
//  Database Wrapper Class
//
//  Created by Steven F. Daniel on 21/11/2017.
//  Copyright © 2017 GENIESOFT STUDIOS. All rights reserved.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MedicineTracker.Models;
using SQLite;

namespace MedicineTracker.Database
{
    public class Database
    {
        static string DatabasePath
        {
            get
            {
                var sqliteFilename = "MedicineTracker.db";
#if __IOS__
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string libraryPath = Path.Combine(documentsPath, "..", "Library");
                var path = Path.Combine(libraryPath, sqliteFilename);
#else
#if __ANDROID__
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var path = Path.Combine(documentsPath, sqliteFilename);
#endif
#endif
                return path;
            }
        }
        static object locker = new object();

        static SQLiteConnection database = new SQLiteConnection(DatabasePath);

        /// <summary>
        /// Create our medicine Item Database table.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public Database()
        {
            // create the tables
            database.CreateTable<MedicineItem>();
        }

        /// <summary>
        /// Gets all of the medicine items from our database.
        /// </summary>
        /// <returns>The items.</returns>
        public IEnumerable<MedicineItem> GetItems()
        {
            // Set a mutual-exclusive lock on our database, while 
            // retrieving items.
            lock (locker)
            {
                return (from i in database.Table<MedicineItem>() select i).ToList();
            }
        }
        /// <summary>
        /// Gets a specific medicine item from the database.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="id">Identifier.</param>
        public MedicineItem GetItem(int id)
        {
            // Set a mutual-exclusive lock on our database, while 
            // retrieving items.
            lock (locker)
            {
                return database.Table<MedicineItem>().FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Saves the medicine item currently being edited.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="item">Item.</param>
        public int SaveItem(MedicineItem item)
        {
            // Set a mutual-exclusive lock on our database, while 
            // saving/updating our medicine item.
            lock (locker)
            {
                if (item.Id != 0)
                {
                    database.Update(item);
                    return item.Id;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        /// <summary>
        /// Deletes a specific medicine item from the database.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="id">Identifier.</param>
        public int DeleteItem(int id)
        {
            // Set a mutual-exclusive lock on our database, while 
            // deleting our medicine item.
            lock (locker)
            {
                return database.Delete<MedicineItem>(id);
            }
        }
    }
}
