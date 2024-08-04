using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Models
{
    public class Fine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public double Amount { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime? DatePaid { get; set; }
        public FineStatus Status { get; set; }
        // Navigation properties
        public virtual User User { get; set; }

        // Method to get fine details
        public string GetFineDetails()
        {
            return $"Fine ID: {Id}, Amount: {Amount:C}, Date Issued: {DateIssued.ToShortDateString()}, Date Paid: {(DatePaid.HasValue ? DatePaid.Value.ToShortDateString() : "Not Paid")}";
        }
    }
}