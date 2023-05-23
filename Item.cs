using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace collectionTest1
{
    public class Item
    {
        public string name;
        public string description;
        public Image image { get; set; }
        public List<string> attributes = new List<string>();
    }
}
