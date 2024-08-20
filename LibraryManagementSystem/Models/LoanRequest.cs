using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a request made by a user to loan an item from the library, including details about the request date, associated item, and user.
    /// </summary>
    public class LoanRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the loan request.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the item being requested for loan.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user making the loan request.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date when the loan request was made.
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the item associated with the loan request. This is a navigation property.
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the loan request. This is a navigation property.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets the title of the item being requested. This is a computed property based on the associated item.
        /// </summary>
        public string Title => Item?.Title;

        /// <summary>
        /// Gets a detailed string representation of the loan request's properties.
        /// </summary>
        /// <returns>A string containing the details of the loan request.</returns>
        public string GetRequestDetails()
        {
            return $"Request ID: {Id}, Item ID: {ItemId}, User ID: {UserId}, Request Date: {RequestDate.ToShortDateString()}";
        }
    }
}