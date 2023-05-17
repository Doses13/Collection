using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collectionTest1
{

    // These will dictate what text boxes show on the "Add Item" page
    // These will be defaulted to "false"
    // When the collection is made, the user will select which attributes they want
    // The attributes they choose will be set to "true" 

    // The attributes with "true" will trigger the "add item" page to turn
    // the specified text boxes to "visible" from their default "collasped" 
    
    // The "Add Item" page will have text boxes coded in for every attribute an item can have
    // But only the attributes set to "true" (as selected by the user) will be visible on the page. 
    public class wantedItemAttributes
    {
        public bool name = false;
        public bool description = false;
        public bool image = false;
        public bool price = false;
        public bool condition = false;
    }
}
