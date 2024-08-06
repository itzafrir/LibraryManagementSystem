using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using System.Windows;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    public partial class AddUpdateItemWindow : Window
    {
        public AddUpdateItemWindow(Item item, ItemService itemService)
        {
            InitializeComponent();
            //DataContext = new AddUpdateItemWindowViewModel(itemService,item, Close);
        }
    }
}