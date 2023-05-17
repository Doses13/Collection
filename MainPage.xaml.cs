using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// Joseph made this comment

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace collectionTest1
{
    public sealed partial class MainPage : Page
    {
        List<Collection> collection_list = new List<Collection>();
        List<Button> gridButtonList = new List<Button>();
        //wantedItemAttributes wantedItems;
        int buttonCounter = 0;
        Windows.Storage.StorageFile file;

        public MainPage()
        {
            this.InitializeComponent();
            //this.SizeChanged += resize;

            //init collections
            Collection rocks = new Collection();
            collection_list.Add(rocks);
            rocks.name = "My Rocks";
            rocks.attributes.Add("Color");

            Item rock1 = new Item();
            rock1.name = "Agate";
            rock1.attributes.Add("Red");

            rocks.items.Add(rock1);

            for (int i = 0; i < collection_list.Count; i++)
            {
                Button colBut = new Button();
                colBut.Content = collection_list[i].name;
                colBut.Width = 200;
                colBut.Height = 50;
                colBut.Margin = new Thickness(10);
                colBut.CornerRadius = new CornerRadius(10);
                colButs.Children.Insert(collection_list.Count-1, colBut);

            }

            List<Button> buttons = new List<Button>();
            Button test = new Button();
            test.Content = "Test";
            test.Margin = new Thickness(10);
            test.Width = 200;
            test.Height = 50;
            buttons.Add(test);
            colButs.Children.Add(buttons[0]);
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
            Home.Visibility = Visibility.Collapsed;
            TextBox itemName = new TextBox();
            addItem.Visibility = Visibility.Visible;

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
            // Toggle home page visible
            Home.Visibility = Visibility.Visible;

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
            if (buttonCounter < 6)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(100, GridUnitType.Auto);
                ItemGrid.RowDefinitions.Add(rowDef);
            }

            // Button and image created from user's input 
            Button addedItemButton = new Button();
            addedItemButton.Content = itemName.Text;
            gridButtonList.Add(addedItemButton);
            BitmapImage bitmapImage = new BitmapImage();
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

                }
            }

            // Add Button and image, shift "Add Item" button over
            ItemGrid.Children.Add(addedItemButton);
            ItemGrid.Children.Add(addedItemImage);
            Grid.SetColumn(addedItemImage, column);
            Grid.SetRow(addedItemImage, row);
            Grid.SetColumn(addedItemButton, column);
            Grid.SetRow(addedItemButton, row);
            Grid.SetColumn(addButton, column);
            Grid.SetRow(addButton, row + 1);
        }

        // "Add Image" Button handler
        public async void addImageInAddItem(object sender, RoutedEventArgs e)
        {

            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            file = await picker.PickSingleFileAsync();
        }
        

        public void NewCollectionFunc(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Collapsed;

        }

        private void resize(object sender, SizeChangedEventArgs e)
        {
            colButs.Height = e.NewSize.Height - 40;
        }
    }
}

