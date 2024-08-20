using LibraryManagementSystem.Utilities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a loan of an item to a user in the library system, including details about the loan date, due date, and loan status.
    /// </summary>
    public class Loan
    {
        /// <summary>
        /// Gets or sets the unique identifier for the loan.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the item being loaned.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who has taken the loan.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date when the loan was made.
        /// </summary>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Gets or sets the due date for returning the loaned item.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the current status of the loan (e.g., Active, Returned, Overdue).
        /// </summary>
        public LoanStatus LoanStatus { get; set; }

        /// <summary>
        /// Gets or sets the item associated with the loan. This is a navigation property.
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the loan. This is a navigation property.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Marks the loan as returned if it is currently active.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the loan is not currently active.</exception>
        public void ReturnLoan()
        {
            if (LoanStatus != LoanStatus.Active)
                throw new InvalidOperationException("Only active loans can be returned.");

            LoanStatus = LoanStatus.Returned;
        }

        /// <summary>
        /// Checks if the loan is overdue.
        /// </summary>
        /// <returns><c>true</c> if the loan is active and the current date is past the due date; otherwise, <c>false</c>.</returns>
        public bool IsOverdue()
        {
            return LoanStatus == LoanStatus.Active && DateTime.Now > DueDate;
        }

        /// <summary>
        /// Gets a detailed string representation of the loan's properties.
        /// </summary>
        /// <returns>A string containing the details of the loan.</returns>
        public string GetLoanDetails()
        {
            return $"Loan ID: {Id}, Item ID: {ItemId}, User ID: {UserId}, Loan Date: {LoanDate.ToShortDateString()}, Due Date: {DueDate.ToShortDateString()}, Status: {LoanStatus}";
        }
    }
}