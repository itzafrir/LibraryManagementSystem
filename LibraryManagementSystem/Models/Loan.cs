using LibraryManagementSystem.Utilities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public LoanStatus LoanStatus { get; set; }

        // Navigation properties
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }

        public void ReturnLoan()
        {
            if (LoanStatus != LoanStatus.Active)
                throw new InvalidOperationException("Only active loans can be returned.");

            LoanStatus = LoanStatus.Returned;
        }

        public bool IsOverdue()
        {
            return LoanStatus == LoanStatus.Active && DateTime.Now > DueDate;
        }

        public string GetLoanDetails()
        {
            return $"Loan ID: {Id}, Item ID: {ItemId}, User ID: {UserId}, Loan Date: {LoanDate.ToShortDateString()}, Due Date: {DueDate.ToShortDateString()}, Status: {LoanStatus}";
        }
    }
}