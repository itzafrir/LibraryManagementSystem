using LibraryManagementSystem.Models;

/// <summary>
/// Represents the state of the item detail view model, capturing the selected item.
/// </summary>
public class ItemDetailViewModelMemento
{
    /// <summary>
    /// Gets the selected item whose details are being viewed.
    /// </summary>
    public Item SelectedItem { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemDetailViewModelMemento"/> class.
    /// </summary>
    /// <param name="selectedItem">The item whose details are being viewed.</param>
    public ItemDetailViewModelMemento(Item selectedItem)
    {
        SelectedItem = selectedItem;
    }
}