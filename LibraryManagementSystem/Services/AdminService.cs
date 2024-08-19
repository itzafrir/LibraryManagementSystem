using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    public class AdminService
    {
        private readonly List<FinePayRequest> _finePayRequests;
        private readonly List<Fine> _fines;

        public AdminService()
        {
            // Dummy data for demonstration purposes
            _finePayRequests = new List<FinePayRequest>();
            _fines = new List<Fine>();
        }

        public List<FinePayRequest> GetFinePayRequests()
        {
            return _finePayRequests;
        }
    }
}
