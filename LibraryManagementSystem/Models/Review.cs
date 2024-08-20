using System;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a review for an item in the library system, including details about the rating, text, and associated user and item.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// Primary key
        /// </summary>
        public int Id { get; set; } 

        /// <summary>
        /// Gets or sets the ID of the item being reviewed.
        ///  Foreign key to Item
        /// </summary>
        public int ItemId { get; set; } 

        /// <summary>
        /// Gets or sets the ID of the user who wrote the review.
        /// Foreign key to User
        /// </summary>
        public int UserId { get; set; } 

        /// <summary>
        /// Gets or sets the rating given in the review, ranging from 1 to 5.
        /// </summary>
        public int Rating { get; set; } 

        /// <summary>
        /// Gets or sets the text of the review.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the date when the review was written.
        /// </summary>
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// Gets or sets the item associated with this review. This is a navigation property.
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Gets or sets the user who wrote this review. This is a navigation property.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Review"/> class with specified user, item, rating, and text.
        /// </summary>
        /// <param name="userId">The ID of the user writing the review.</param>
        /// <param name="itemId">The ID of the item being reviewed.</param>
        /// <param name="rating">The rating given in the review (1 to 5).</param>
        /// <param name="text">The text of the review.</param>
        public Review(int userId, int itemId, int rating, string text)
        {
            UserId = userId;
            ItemId = itemId;
            Rating = rating;
            Text = text;
            ReviewDate = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Review"/> class for Entity Framework.
        /// </summary>
        public Review()
        {
        }
    }
}