using System;
using System.Windows;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    public partial class AdminView : Window
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;

        public AdminView(ItemService itemService, UserService userService)
        {
            InitializeComponent();
            _itemService = itemService;
            _userService = userService;
        }
    }
}