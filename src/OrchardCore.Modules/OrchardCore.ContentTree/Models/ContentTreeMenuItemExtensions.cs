using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Models
{
    public static class ContentTreeMenuItemExtensions
    {
        public static MenuItem GetMenuItemById(this MenuItem sourceItem, string id)
        {
            var tempStack = new Stack<MenuItem>(new MenuItem[] { sourceItem });            

            while (tempStack.Any())
            {
                // evaluate first node
                MenuItem item = tempStack.Pop();
                if (item.UniqueId.Equals(id, StringComparison.OrdinalIgnoreCase)) return item;

                // not that one; continue with the rest.
                foreach (var i in item.Items) tempStack.Push(i);
            }

            //not found
            return null;
        }

        // return boolean so that caller can check for success
        public static bool RemoveMenuItem(this MenuItem sourceItem, MenuItem itemToRemove)
        {
            var tempStack = new Stack<MenuItem>(new MenuItem[] { sourceItem });

            while (tempStack.Any())
            {
                // evaluate first
                MenuItem item = tempStack.Pop();
                if (item.Items.Contains(itemToRemove))
                {
                    item.Items.Remove(itemToRemove);
                    return true; //success
                }

                // not that one. continue
                foreach (var i in item.Items) tempStack.Push(i);
            }

            // failure
            return false;
        }
        
        
        public static bool InsertMenuItem(this MenuItem rootMenuItem, MenuItem itemToInsert,
                    MenuItem destinationItem, int position)
        {
            var tempStack = new Stack<MenuItem>(new MenuItem[] { rootMenuItem });
            while (tempStack.Any())
            {
                // evaluate first
                MenuItem node = tempStack.Pop();
                if (node.Equals(destinationItem))
                {
                    node.Items.Insert(position, itemToInsert);
                    return true; // success
                }

                // not that one. continue
                foreach (var n in node.Items) tempStack.Push(n);
            }

            // failure
            return false;
        }
    }
}
