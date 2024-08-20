using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a payment request for a fine, including details about the associated fine, user, and request date.
    /// </summary>
    public class FinePayRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the payment request.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the fine ID associated with this payment request.
        /// </summary>
        public int FineId { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with this payment request.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date when the payment request was made.
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the fine associated with this payment request. This is a navigation property.
        /// </summary>
        public virtual Fine Fine { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this payment request. This is a navigation property.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets a detailed string representation of the payment request's properties.
        /// </summary>
        /// <returns>A string containing the details of the payment request.</returns>
        public string GetRequestDetails()
        {
            return $"Request ID: {Id}, Fine ID: {FineId}, User ID: {UserId}, Request Date: {RequestDate.ToShortDateString()}";
        }
    }
}
