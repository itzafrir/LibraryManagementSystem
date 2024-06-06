using System;

namespace LibraryManagementSystem.Models
{
    public class LoanRequest
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }

        // Method to get the request details
        public string GetRequestDetails()
        {
            return $"Request ID: {Id}, Item ID: {ItemId}, User ID: {UserId}, Request Date: {RequestDate.ToShortDateString()}";
        }
    }
}