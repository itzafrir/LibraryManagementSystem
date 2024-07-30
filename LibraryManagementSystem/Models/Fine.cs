using System;

namespace LibraryManagementSystem.Models
{
    public class Fine
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime? DatePaid { get; set; }

        // Navigation properties
        public virtual User User { get; set; }

        // Method to get fine details
        public string GetFineDetails()
        {
            return $"Fine ID: {Id}, Amount: {Amount:C}, Date Issued: {DateIssued.ToShortDateString()}, Date Paid: {(DatePaid.HasValue ? DatePaid.Value.ToShortDateString() : "Not Paid")}";
        }
    }
}