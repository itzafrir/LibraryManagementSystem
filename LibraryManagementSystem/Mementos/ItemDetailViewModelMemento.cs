using LibraryManagementSystem.Models;

public class ItemDetailViewModelMemento
{
    public Item SelectedItem { get; }

    public ItemDetailViewModelMemento(Item selectedItem)
    {
        SelectedItem = selectedItem;
    }
}