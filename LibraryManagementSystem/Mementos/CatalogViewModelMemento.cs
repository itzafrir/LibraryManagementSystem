using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Utilities
{
    public class CatalogViewModelMemento
    {
        public string SearchTerm { get; set; }
        public ItemType? SelectedItemType { get; set; }

        public CatalogViewModelMemento(string searchTerm, ItemType? selectedItemType)
        {
            SearchTerm = searchTerm;
            SelectedItemType = selectedItemType;
        }
    }
}