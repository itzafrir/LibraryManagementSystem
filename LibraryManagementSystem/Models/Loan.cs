using LibraryManagementSystem.Utilities.Enums;
using System;

namespace LibraryManagementSystem.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public LoanStatus LoanStatus { get; set; }

        // Method to return the loan
        public void ReturnLoan()
        {
            LoanStatus = LoanStatus.Returned;
        }

        // Method to check if the loan is overdue
        public bool IsOverdue()
        {
            return LoanStatus == LoanStatus.Active && DateTime.Now > DueDate;
        }

        // Method to get the loan details
        public string GetLoanDetails()
        {
            return $"Loan ID: {Id}, Item ID: {ItemId}, User ID: {UserId}, Loan Date: {LoanDate.ToShortDateString()}, Due Date: {DueDate.ToShortDateString()}, Status: {LoanStatus}";
        }
    }
}