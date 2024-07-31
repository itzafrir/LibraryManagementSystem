using System;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Models
{
    public class FinePayRequest
    {
        public int Id { get; set; }
        public int FineId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public FinePayRequestStatus Status { get; set; } // Enum for status

        // Navigation properties
        public virtual Fine Fine { get; set; }
        public virtual User User { get; set; }

        public string GetRequestDetails()
        {
            return $"Request ID: {Id}, Fine ID: {FineId}, User ID: {UserId}, Request Date: {RequestDate.ToShortDateString()}, Status: {Status}";
        }
    }
}