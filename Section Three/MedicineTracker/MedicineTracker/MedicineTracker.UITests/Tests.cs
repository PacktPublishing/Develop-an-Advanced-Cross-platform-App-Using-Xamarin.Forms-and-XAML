using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace MedicineTracker.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        string entryCellPlatformClassName;

        public Tests(Platform platform)
        {
            this.platform = platform;
            entryCellPlatformClassName = platform == Platform.iOS ? "UITextField" : "EntryCellEditText";
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        // Test - Creating a new medication entry to the database
        public void CreateNewMedicationEntry()
        {
            // Wait for our main screen to appear
            var navigationBarTitle = "Medicine Items Listing";
            var mainScreen = app.WaitForElement(x => x.Marked(navigationBarTitle));

            // Assert main screen is shown after the Medications Items Listing is displayed.
            Assert.IsTrue(mainScreen.Any(), navigationBarTitle + " screen wasn't shown after launch.");

            // Tap Add to add a new medicine item
            app.Tap(x => x.Marked("Add"));

            // Wait for the Adding Medicine Details screen to appear
            var newMedicineBarTitle = "Adding Medicine Details";
            var newMedicineScreen = app.WaitForElement(x => x.Marked(newMedicineBarTitle));

            // Assert newMedicineScreen is shown after clicking on the Add button
            Assert.IsTrue(newMedicineScreen.Any(), newMedicineBarTitle + "screen was not shown after tapping the Add button.");

            // Populate our Medicine Details within the Medicine Details screen
            PopulateMedicineDetails();

            // Populate our Dosage Details within the Medicine Details screen
            PopulateDosageDetails();

            // Tap Save to exit the Adding Medicine Details screen
            app.Tap(x => x.Marked("Save"));

            // Wait for the save medicine item screen to appear
            var saveDialog = app.WaitForElement(x => x.Marked("Save Medicine Item"));

            // Assert save medicine item screen is shown after clicking save
            Assert.IsTrue(saveDialog.Any(), "Save Medicine Item screen was not shown after clicking on Save");

            // Tap OK button to save the medicine item
            app.Tap(x => x.Marked("OK"));

            // Wait for main medicine items listing screen to appear
            var medicineItemsListing = app.WaitForElement(x => x.Marked("Medicine Items Listing"));

            // Assert main medicine items listing is shown after saving a new Medicine Item
            Assert.IsTrue(medicineItemsListing.Any(), "Main screen was not shown after saving a new Medicine Item");
        }

        [Test]
        // Test - Validate for missing brand name and description
        public void Validate_CheckForMissingBrandNameDesc()
        {
            // Wait for main screen to appear
            var mainScreen = app.WaitForElement(x => x.Marked("Medicine Items Listing"));

            // Assert main screen is shown after saving a new Medicine Item
            Assert.IsTrue(mainScreen.Any(), "Main screen was not shown after saving a new Medicine Item");

            // Tap Add to add a new medicine item
            app.Tap(x => x.Marked("Add"));

            // Wait for the Adding Medicine Details screen to appear
            var newMedicineBarTitle = "Adding Medicine Details";
            var newMedicineScreen = app.WaitForElement(x => x.Marked(newMedicineBarTitle));

            // Assert newMedicineScreen is shown after clicking on the Add button
            Assert.IsTrue(newMedicineScreen.Any(), newMedicineBarTitle + "screen was not shown after tapping the Add button.");

            // Populate our Medicine Details within the Medicine Details screen
            PopulateMedicineDetails();

            // Populate our Dosage Details within the Medicine Details screen
            PopulateDosageDetails();

            // Clear out our Brand Name EntryCell
            app.ClearText(x => x.Class(entryCellPlatformClassName).Index(0));
            app.DismissKeyboard();

            // Clear out our Description EntryCell
            app.ClearText(x => x.Class(entryCellPlatformClassName).Index(1));
            app.DismissKeyboard();

            // Tap Save to save the new medicine item
            app.Tap(x => x.Marked("Save"));

            // Wait for the save medicine item screen to appear
            var saveDialog = app.WaitForElement(x => x.Marked("Save Medicine Item"));

            // Assert save medicine item screen is shown after clicking save
            Assert.IsTrue(saveDialog.Any(), "Save Medicine Item screen was not shown after clicking on Save");

            // Tap OK button to save the medicine item
            app.Tap(x => x.Marked("OK"));

            // Wait for the error screen to appear
            var errorScreen = app.WaitForElement(x => x.Marked("Error"));

            // Assert error screen is shown after saving a new Medicine Item
            Assert.IsTrue(errorScreen.Any(), "Brand Name and Description are required screen was not shown.");

            // Tap OK button to dismiss the dialog
            app.Tap(x => x.Marked("OK"));
        }

        [Test]
        // Test - Validate changes not being saved to the database
        public void Validate_DontSaveChanges()
        {
            // Wait for main screen to appear
            var mainScreen = app.WaitForElement(x => x.Marked("Medicine Items Listing"));

            // Assert main screen is shown after saving a new Medicine Item
            Assert.IsTrue(mainScreen.Any(), "Main screen was not shown after saving a new Medicine Item");

            // Tap Add to add a new medicine item
            app.Tap(x => x.Marked("Add"));

            // Wait for the Adding Medicine Details screen to appear
            var newMedicineBarTitle = "Adding Medicine Details";
            var newMedicineScreen = app.WaitForElement(x => x.Marked(newMedicineBarTitle));

            // Assert newMedicineScreen is shown after clicking on the Add button
            Assert.IsTrue(newMedicineScreen.Any(), newMedicineBarTitle + "screen was not shown after tapping the Add button.");

            // Populate our Medicine Details within the Medicine Details screen
            PopulateMedicineDetails();

            // Populate our Dosage Details within the Medicine Details screen
            PopulateDosageDetails();

            // Tap Save to save the new medicine item
            app.Tap(x => x.Marked("Save"));

            // Wait for the save medicine item screen to appear
            var saveDialog = app.WaitForElement(x => x.Marked("Save Medicine Item"));

            // Assert save medicine item screen is shown after clicking save
            Assert.IsTrue(saveDialog.Any(), "Save Medicine Item screen was not shown after clicking on Save");

            // Tap Cancel button to discard changes
            app.Tap(x => x.Marked("Cancel"));

            // Wait for main medicine items listing screen to appear
            var medicineItemsListing = app.WaitForElement(x => x.Marked("Medicine Items Listing"));

            // Assert main medicine items listing is shown after saving a new Medicine Item
            Assert.IsTrue(medicineItemsListing.Any(), "Main screen was not shown after saving a new Medicine Item");
        }

        // Populate entry cell fields for our Medicine Details Section
        void PopulateMedicineDetails()
        {
            // Enter in some text for our Brand Name EntryCell
            app.ClearText(x => x.Class(entryCellPlatformClassName).Index(0));
            app.EnterText(x => x.Class(entryCellPlatformClassName).Index(0), "Panadol");
            app.DismissKeyboard();

            // Enter in some text for our Description EntryCell
            app.EnterText(x => x.Class(entryCellPlatformClassName).Index(1),
            "Taken to help with headache");
            app.DismissKeyboard();

            // Clear the default text and enter in some text for our Side Effects EntryCell
            app.ClearText(x => x.Class(entryCellPlatformClassName).Index(2));
            app.EnterText(x => x.Class(entryCellPlatformClassName).Index(2), "None specified");
            app.DismissKeyboard();
        }

        // Populate entry cell fields for our Dosage Details Section
        void PopulateDosageDetails()
        {
            // Enter in some text for our Dosage Details EntryCell
            app.ClearText(x => x.Class(entryCellPlatformClassName).Index(3));
            app.EnterText(x => x.Class(entryCellPlatformClassName).Index(3), "2 tablets, 8 max per day");
            app.DismissKeyboard();

            // Tap into the Date Dose Taken EntryCell (fourth EntryCell)
            if (platform == Platform.iOS)
            {
                // Tap Done on the Date Dose Taken picker
                app.Tap(x => x.Class(entryCellPlatformClassName).Index(4));
                app.Tap(x => x.Marked(platform == Platform.iOS ? "Done" : "OK"));
            }

            // Tap Done on the Time Dose Taken picker
            if (platform == Platform.iOS)
            {
                // Tap Done on the Date Dose Taken picker
                app.Tap(x => x.Class(entryCellPlatformClassName).Index(5));
                app.Tap(x => x.Marked(platform == Platform.iOS ? "Done" : "OK"));
            }
        }
    }
}