using System;

namespace LibraryManagementSystem.Models
{
    public class Review
    {
        public int Id { get; set; } // Primary key
        public int ItemId { get; set; } // Foreign key to Item
        public int UserId { get; set; } // Foreign key to User
        public int Rating { get; set; } // Rating from 1 to 5
        public string Text { get; set; }
        public DateTime ReviewDate { get; set; }

        public virtual Item Item { get; set; }
        public virtual User User { get; set; }

        public Review(int userId, int itemId, int rating, string text)
        {
            UserId = userId;
            ItemId = itemId;
            Rating = rating;
            Text = text;
            ReviewDate = DateTime.Now;
        }

        // Parameterless constructor for EF
        public Review()
        {
        }
    }
}