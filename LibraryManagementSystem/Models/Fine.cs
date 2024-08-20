using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a fine imposed on a user for a library item, including details about the amount, issue date, payment status, and related user and item.
    /// </summary>
    public class Fine
    {
        /// <summary>
        /// Gets or sets the unique identifier for the fine.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the fine.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the item ID associated with the fine.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the amount of the fine.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the date when the fine was issued.
        /// </summary>
        public DateTime DateIssued { get; set; }

        /// <summary>
        /// Gets or sets the date when the fine was paid. This can be null if the fine is not yet paid.
        /// </summary>
        public DateTime? DatePaid { get; set; }

        /// <summary>
        /// Gets or sets the current status of the fine (e.g., Paid, Unpaid).
        /// </summary>
        public FineStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the fine. This is a navigation property.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets a detailed string representation of the fine's properties.
        /// </summary>
        /// <returns>A string containing the details of the fine.</returns>
        public string GetFineDetails()
        {
            return $"Fine ID: {Id}, Amount: {Amount:C}, Date Issued: {DateIssued.ToShortDateString()}, Date Paid: {(DatePaid.HasValue ? DatePaid.Value.ToShortDateString() : "Not Paid")}, Status: {Status}";
        }
    }
}