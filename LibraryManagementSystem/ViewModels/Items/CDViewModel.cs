﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class CDViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private CD _cd;

        public CD CD
        {
            get => _cd;
            set => SetProperty(ref _cd, value);
        }

        public CDViewModel(CD cd, ItemService itemService)
        {
            _itemService = itemService;
            _cd = cd;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save()
        {
            if (IsValid())
            {
                if (_cd.Id == 0)
                {
                    _itemService.AddItem(_cd);
                }
                else
                {
                    _itemService.UpdateItem(_cd);
                }
                CloseAction();
            }
            else
            {
                ShowValidationError();
            }
        }

        private void Cancel()
        {
            CloseAction();
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_cd.Title) &&
                   !string.IsNullOrWhiteSpace(_cd.Artist) &&
                   !string.IsNullOrWhiteSpace(_cd.Genre) &&
                   _cd.Duration != default &&
                   !string.IsNullOrWhiteSpace(_cd.Label) &&
                   _cd.ReleaseDate != default &&
                   _cd.TotalCopies > 0 &&
                   _cd.AvailableCopies >= 0 &&
                   _cd.TotalCopies >= _cd.AvailableCopies;
        }

        private void ShowValidationError()
        {
            MessageBox.Show("Please fill in all fields and ensure that Total Copies is greater than or equal to Available Copies.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public Action CloseAction { get; set; }
    }
}
