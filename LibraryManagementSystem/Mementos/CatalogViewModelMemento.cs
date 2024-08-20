using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Utilities
{
    /// <summary>
    /// Represents the state of the catalog view model, capturing search criteria and selected item type.
    /// </summary>
    public class CatalogViewModelMemento
    {
        /// <summary>
        /// Gets or sets the search term used in the catalog search.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the selected item type filter in the catalog.
        /// </summary>
        public ItemType? SelectedItemType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogViewModelMemento"/> class.
        /// </summary>
        /// <param name="searchTerm">The search term used in the catalog search.</param>
        /// <param name="selectedItemType">The item type filter selected in the catalog. Can be null.</param>
        public CatalogViewModelMemento(string searchTerm, ItemType? selectedItemType)
        {
            SearchTerm = searchTerm;
            SelectedItemType = selectedItemType;
        }
    }
}