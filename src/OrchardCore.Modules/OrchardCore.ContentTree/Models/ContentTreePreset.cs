using System;
using System.Collections.Generic;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Models
{
    public class ContentTreePreset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public List<MenuItem> MenuItems { get; } = new List<MenuItem>();


        public MenuItem GetMenuItemById(string id)
        {
            foreach (var menuItem in MenuItems)
            {
                var found = menuItem.GetMenuItemById(id);
                if (found != null)
                {
                    return found;
                }
            }

            // not found
            return null;
        }

        public bool RemoveMenuItem(MenuItem itemToRemove)
        {
            if (MenuItems.Contains(itemToRemove)) // todo: avoid this check by having a single TreeNode as a property of the content tree preset.
            {
                MenuItems.Remove(itemToRemove);
                return true; // success
            }
            else
            {
                foreach (var firstLevelMenuItem in MenuItems)
                {
                    if (firstLevelMenuItem.RemoveMenuItem(itemToRemove))
                    {
                        return true; // success
                    }
                }                
            }

            return false; // failure
        }

        public bool InsertMenuItemAt(MenuItem menuItemToInsert, MenuItem destinationMenuItem, int position)
        {
            if (menuItemToInsert == null)
            {
                throw new ArgumentNullException("menuItemToInsert");
            }

            // insert the node at the destination node
            if (destinationMenuItem == null)
            {
                MenuItems.Insert(position, menuItemToInsert);
                return true; // success
            }
            else
            {
                foreach (var firstLevelMenuItem in MenuItems)
                {
                    if (firstLevelMenuItem.InsertMenuItem(menuItemToInsert, destinationMenuItem, position))
                    {
                        return true; // success
                    }
                }                
            }
            return false; // failure
        }

    }
}
