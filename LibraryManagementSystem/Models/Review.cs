using System;

namespace LibraryManagementSystem.Models
{
    public class Review
    {
        public int Id { get; set; } // Primary key
        public int UserId { get; set; }
        public int Rating { get; set; } // Rating from 1 to 5
        public string Text { get; set; }
        public DateTime ReviewDate { get; set; }

        public Review(int userId, int rating, string text)
        {
            UserId = userId;
            Rating = rating;
            Text = text;
            ReviewDate = DateTime.Now;
        }
    }
}