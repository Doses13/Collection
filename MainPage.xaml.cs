using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Joseph made this comment

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace collectionTest1
{
    public sealed partial class MainPage : Page
    {
        List<Collection> collection_list = new List<Collection>();

        public MainPage()
        {
            this.InitializeComponent();

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
                colBut.Margin = new Thickness(10);
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

        public void NewCollectionFunc(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Collapsed;

        }

    }
}

