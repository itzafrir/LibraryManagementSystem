using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class LoanRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }

        // Navigation properties
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }

        // Computed property
        public string Title => Item?.Title;
        // Method to get the request details
        public string GetRequestDetails()
        {
            return $"Request ID: {Id}, Item ID: {ItemId}, User ID: {UserId}, Request Date: {RequestDate.ToShortDateString()}";
        }
    }
}