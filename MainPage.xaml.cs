﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Storage;

// Joseph made this comment

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace collectionTest1
{
    public sealed partial class MainPage : Page
    {
        enum screens{
            Home,
            AddItem,
            AddCol,
            Single,
        }
        // This list stores all the collections that the user currently has.
        // Gui items should only be added for collections and items present in it. JB    
        List<Collection> collectionList = new List<Collection>();

        //This is a temp collection that will get called new whenever we need to create
        //a new collection. CK
        Collection collection;

        // This variable stores the active collection i.e. the collection whos items are
        // being shown on the screen.
        // To change the active collection the refresh function should be ran
        // which will automatically update this variable and the items shown on the screen.
        // DO NOT MODIFY VALUE or the wrong collection will be displayed. JB
        int activeCollection = 0;

        //wantedItemAttributes wantedItems;
        int buttonCounter = 0;

        Windows.Storage.StorageFile file;


        //Navigation Variables
        screens currentScreen = screens.Home; //Home screen

        BitmapImage bitmapImage = new BitmapImage();


        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += resize;

            /* Init collections
            Collection rocks = new Collection();
            collectionList.Add(rocks);
            rocks.name = "My Rocks";
            rocks.attributes.Add("Color");
            Item rock1 = new Item();
            rock1.name = "Agate";
            rock1.attributes.Add("Red");
            rocks.items.Add(rock1);//*/

            refresh(activeCollection);
        }


        // This is work in progress code I am submitting very late at night
        // What it will do:
        //
        // Show textboxes on the "add item" page according to which items the user selelected (if they want price, condition, it will show text boxes for those)
        // Take the image and name submitted by users, and create a new button on the grid using those
        // Dynamically increase the grid's columns and rows depending on how many existing items there are

        // The visibility of the text boxes will depend on what attributes are selected by the user when making a collection.
        // When we code the "Add collection" functionality, I will add the functionality to turn each attribute to "true" depending on what the user selects, which will decide which text boxes
        // are displayed when adding an item.
        // This will be in the form of the "WantedItemAttributes" class boolean variables being turned "true" 
        // If the user selects that they want the "Condition" attribute for this collection, the "Condition" variable in "WantedItemAttributes" will be turned true
        // When the "Condition" attribute is true, the textbox for "Condition" will be turned visible on the "Add item" page.
        public void newItemFunc(object sender, RoutedEventArgs e)
        {
            changeScreen(screens.AddItem);
                        

            // Toggle textboxes visible if attributes were selected when making collection
            /*
           if (wantedItems.price == true)
            {
                itemPrice.Visibility = Visibility.Visible;
            }
           if(wantedItems.description == true)
            {
                itemDescription.Visibility = Visibility.Visible;
            }
           if(wantedItems.condition == true)
            {
                itemCondition.Visibility = Visibility.Visible;
            }
            */

        }

        public async void newItemConfirm(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addItemName.Text) && !string.IsNullOrEmpty(addItemDescription.Text))
            {
                /*for (int i = 0; i < collectionList[activeCollection].attributes.Count(); i++)
                {
                    TextBox textBox = new TextBox();
                }*/

                changeScreen(screens.Home);

                // Calculate where this button should go on the grid
                int column = buttonCounter / 6;
                int row = buttonCounter % 6;
                buttonCounter++;

                // Check if an additional column is needed
                if (row == 0)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    col.Width = new GridLength(100, GridUnitType.Auto);
                    ItemGrid.ColumnDefinitions.Add(col);
                }

                // Check if item needs to go to next row 
                if (buttonCounter < 8)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = new GridLength(100, GridUnitType.Auto);
                    ItemGrid.RowDefinitions.Add(rowDef);
                }

                // Button and image created from user's input 
                /*Button addedItemButton = new Button();
                addedItemButton.Content = addItemName.Text;
                addedItemButton.VerticalContentAlignment = VerticalAlignment.Bottom;
                gridButtonList.Add(addedItemButton);*/

                Image addedItemImage = new Image();
                // Convert image to bitmapImage to use with grid
                if (file != null)
                {
                    using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {

                        await bitmapImage.SetSourceAsync(fileStream);
                        addedItemImage.Source = bitmapImage;
                        addedItemImage.Width = 100;
                        addedItemImage.Height = 100;
                        addedItemImage.PointerPressed += AddedItemImage_PointerPressed;

                    }
                }
                else
                {
                    bitmapImage.UriSource = new Uri("ms-appx:///Assets/placeHolder.png", UriKind.Absolute);
                    addedItemImage.Source = bitmapImage;
                    addedItemImage.Opacity = 0.5;
                    addedItemImage.Width = 100;
                    addedItemImage.Height = 100;
                    addedItemImage.PointerPressed += AddedItemImage_PointerPressed;
                }

                // Add Button and image, shift "Add Item" button over
               // ItemGrid.Children.Add(addedItemButton);
                ItemGrid.Children.Add(addedItemImage);
                Grid.SetColumn(addedItemImage, column);
                Grid.SetRow(addedItemImage, row);
                //Grid.SetColumn(addedItemButton, column);
                //Grid.SetRow(addedItemButton, row);
                Grid.SetColumn(addButton, column + buttonCounter);
                Grid.SetRow(addButton, row);
            }
        }

        private void saveAddItem(Item newlyAddedItem)
        {
            List<Item> items = new List<Item>();
            for(int i = 0; i < collectionList[activeCollection].items.Count(); i++)
            {
                items.Add(collectionList[activeCollection].items[i]);
            }

            items.Add(newlyAddedItem);
            collectionList[activeCollection].items = items;
        }

        // Event handler for when user clicks on an image of an item in the collection
        private void AddedItemImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            changeScreen(screens.Single);

            Item tempItem = new Item();
            tempItem.name = addItemName.Text;
            tempItem.description = addItemDescription.Text;
            tempItem.image.Source = bitmapImage;
            saveAddItem(tempItem);

            // Copies the text that user inputted whren creating the item  
        }


        // "Add Image" Button handler
        public async void addImageInAddItem(object sender, RoutedEventArgs e)
        {
            // Create a file picker on button-press
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                // Open file picker with settings default to typical image selecting (View mode larger, starting in 
                // Picture directory of computer)
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            // Looks for JPG, JPEG, or PNG
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            file = await picker.PickSingleFileAsync();
        }

        // This function will display a particular collections items on the screen.
        // It will also update the collections displayed on the left.
        // To be called when items are added, removed, or modified in a collection internally
        // This function takes a parameter to choose what collection is the active collection. JB
        private void refresh(int collectionNumber)
        {
            activeCollection = collectionNumber; // Change active collection
            
            // Manage displaying of items

            // Remove all current items from screen
            // TODO: code to remove all items from screen

            // Add new items

            // TODO: code for adding items to screen
            

            // Update collections displayed on left

            // Save add collection button
            Button addColBut = (Button)colButs.Children.Last();

            // Remove existing colleciton buttons
            colButs.Children.Clear();

            // Update screen to display all the collections.
            for (int i = 0; i < collectionList.Count; i++)
            {
                Button colBut = new Button();
                colBut.Content = collectionList[i].name;
                colBut.Width = 200;
                colBut.Height = 50;
                colBut.Margin = new Thickness(10);
                colBut.CornerRadius = new CornerRadius(10);
                colBut.Click += collectionButtons;
                colButs.Children.Insert(collectionList.Count - 1, colBut);
            }

            // Add back 'add collection button'
            colButs.Children.Add(addColBut);
        }

        // This function is to consolidate navigation
        // Takes in the screen to change to and tries to make that change
        // Returns 1 if the requested screen change is invalid
        private int changeScreen(screens screen)
        {
            if(currentScreen == screens.Home && screen == screens.AddItem) // Home -> Add Item
            {
                Home.Visibility = Visibility.Collapsed;
                addItem.Visibility = Visibility.Visible;
                backButtonBar.Visibility = Visibility.Visible;
                singleItemView.Visibility = Visibility.Collapsed;
                currentScreen = screens.AddItem;
                return 0;
            }

            if(currentScreen == screens.Home && screen == screens.AddCol) // Home -> Add Collection
            {
                Home.Visibility = Visibility.Collapsed;
                addItem.Visibility = Visibility.Collapsed;
                addCollection.Visibility = Visibility.Visible;
                backButtonBar.Visibility = Visibility.Visible;
                singleItemView.Visibility = Visibility.Collapsed;
                currentScreen = screens.AddCol;
                return 0;
            }

            if(currentScreen == screens.Home && screen == screens.Single) // Home -> Single Item View
            {

                Home.Visibility = Visibility.Collapsed;
                addItem.Visibility = Visibility.Collapsed;
                backButtonBar.Visibility = Visibility.Visible;
                addCollection.Visibility = Visibility.Collapsed;
                singleItemView.Visibility = Visibility.Visible;
                currentScreen = screens.Single;
                return 0;
            }

            if(currentScreen == screens.AddItem && screen == screens.Home) // Add Item -> Home
            {
                Home.Visibility = Visibility.Visible;
                addItem.Visibility = Visibility.Collapsed;
                backButtonBar.Visibility = Visibility.Collapsed;
                singleItemView.Visibility = Visibility.Collapsed;
                currentScreen = screens.Home;
                return 0;
            }

            if(currentScreen == screens.AddCol && screen == screens.Home) // Add Collection -> Home
            {
                Home.Visibility = Visibility.Visible;
                addItem.Visibility = Visibility.Visible;
                addCollection.Visibility = Visibility.Collapsed;
                backButtonBar.Visibility = Visibility.Collapsed;
                singleItemView.Visibility = Visibility.Collapsed;
                currentScreen = screens.Home;
                return 0;
            }

            if(currentScreen == screens.Single && screen == screens.Home) // SIngle Item View -> Home
            {
                Home.Visibility = Visibility.Visible;
                addItem.Visibility = Visibility.Collapsed;
                backButtonBar.Visibility = Visibility.Collapsed;
                addCollection.Visibility = Visibility.Collapsed;
                singleItemView.Visibility = Visibility.Collapsed;
                currentScreen = screens.Home;
                return 0;
            }

            return 1;
        }

        private void backButtonPress(object sender, RoutedEventArgs e)
        {
            changeScreen(screens.Home);
        }

        // Called when a collection button is pressed.
        private void collectionButtons(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < collectionList.Count; i++)
            {
                if (((Button)sender).Content.ToString() == collectionList[i].name)
                {
                    refresh(i);
                }
            }
        }
        public void NewCollectionFunc(object sender, RoutedEventArgs e)
        {
            changeScreen(screens.AddCol);
            // to whoever is working on this make sure that collections have unique names pls. JB
            collection = new Collection();
            collectionList.Add(collection);
        }

        private void resize(object sender, SizeChangedEventArgs e)
        {
            colButs.Height = e.NewSize.Height - 40;
        }

        public void addAttribute(object sender, RoutedEventArgs e)
        {
            if(attText.Text!=null || attText.Text=="")
            {
                collectionList.Last().attributes.Add(attText.Text);
                attText.Text = "";
            }
        }

        public void NewCollectionConfirm(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(colName.Text))
            {
                collectionList.Last().name = colName.Text;
                colName.Text = "";
                attText.Text = "";
                changeScreen(screens.Home);
                refresh(activeCollection + 1);
            }
            else 
            { 
                colRequiredField.Visibility = Visibility.Visible;
            }
        }

        private async void exportCol(object sender, RoutedEventArgs e)
        {
            if (collectionList.Count > 0)
            {
                var picker = new Windows.Storage.Pickers.FileSavePicker();
                picker.FileTypeChoices.Add("Collection", new List<string> { ".col" });
                Windows.Storage.StorageFile file = await picker.PickSaveFileAsync();
                var serializer = new XmlSerializer(collectionList[activeCollection].GetType());
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, collectionList[activeCollection]);
                await FileIO.WriteTextAsync(file, writer.ToString());
                writer.Close();
            }
            else
            {
                //TODO: make message box pop-up saying that there are no colection to export.
            }
        }

        private async void importCol(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.FileTypeFilter.Add(".col"); //This is necessary
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                Collection newCol = new Collection();
                String data = await Windows.Storage.FileIO.ReadTextAsync(file);
                var serializer = new XmlSerializer(newCol.GetType());
                StringReader reader = new StringReader(data);
                newCol = (Collection)serializer.Deserialize(reader);
                collectionList.Add(newCol);
                refresh(activeCollection);
            }
        }

        private void clickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        public void singleViewEditClick(object sender, RoutedEventArgs e)
        {
            singleViewToggleEditing();
        }

        public void singleViewSaveClick(object sender, RoutedEventArgs e)
        {
            singleViewToggleEditing();

        }

        //Single View Edit and Save Helper
        private void singleViewToggleEditing()
        {
            singleViewName.IsEnabled = !singleViewName.IsEnabled;
            singleViewDescription.IsEnabled = !singleViewDescription.IsEnabled;
            foreach (TextBox attribute in attributePanel.Children)
            {
                attribute.IsEnabled = !attribute.IsEnabled;
            }

            if (editItem.Visibility == Visibility.Visible)
            {
                editItem.Visibility = Visibility.Collapsed;
                saveItemChanges.Visibility = Visibility.Visible;
            }
            else
            {
                editItem.Visibility = Visibility.Visible;
                saveItemChanges.Visibility = Visibility.Collapsed;
            }
        }
    }
}

