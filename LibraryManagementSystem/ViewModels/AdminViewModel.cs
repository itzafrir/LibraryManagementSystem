using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly Action _navigateHome;

        private FinePayRequest _selectedFinePayRequest;
        public ObservableCollection<FinePayRequest> FinePayRequests { get; }
        public FinePayRequest SelectedFinePayRequest
        {
            get => _selectedFinePayRequest;
            set => SetProperty(ref _selectedFinePayRequest, value);
        }

        public ICommand ApproveFinePayRequestCommand { get; }
        public ICommand RejectFinePayRequestCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        public event EventHandler RequestClose;

        public AdminViewModel(ItemService itemService, UserService userService, Action navigateHome)
        {
            _itemService = itemService;
            _userService = userService;
            _navigateHome = navigateHome;

            FinePayRequests = new ObservableCollection<FinePayRequest>(_userService.GetFinePayRequests());

            ApproveFinePayRequestCommand = new RelayCommand(ApproveFinePayRequest);
            RejectFinePayRequestCommand = new RelayCommand(RejectFinePayRequest);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
        }

        private void ApproveFinePayRequest()
        {
            if (SelectedFinePayRequest != null)
            {
                _userService.ApproveFinePayRequest(SelectedFinePayRequest);
                FinePayRequests.Remove(SelectedFinePayRequest);
            }
        }

        private void RejectFinePayRequest()
        {
            if (SelectedFinePayRequest != null)
            {
                _userService.RejectFinePayRequest(SelectedFinePayRequest);
                FinePayRequests.Remove(SelectedFinePayRequest);
            }
        }

        private void NavigateHome()
        {
            _navigateHome?.Invoke();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
