using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;

namespace LibraryManagementSystem.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ISBN { get; set; }

        public double Rating { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        public string Publisher { get; set; }

        public string Description { get; set; }

        public ObservableCollection<Review> Reviews { get; set; }

        private int _totalCopies;
        public int TotalCopies
        {
            get => _totalCopies;
            set
            {
                int difference = value - _totalCopies;

                if (difference > 0)
                {
                    // Adding more copies
                    AvailableCopies += difference;
                }
                else if (difference < 0)
                {
                    // Removing copies
                    if (AvailableCopies + difference < 0)
                    {
                        MessageBox.Show("Cannot reduce total copies because it would result in negative number of copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    AvailableCopies += difference; // difference is negative, so it subtracts
                }

                _totalCopies = value;
            }
        }

        private int _availableCopies;
        public int AvailableCopies
        {
            get => _availableCopies;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Available copies cannot be negative.");
                }
                _availableCopies = value;
            }
        }

        // Property to calculate the average rating of the item
        public double AverageRating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                    return 0;
                return Reviews.Average(r => r.Rating);
            }
        }

        // Constructor
        public Item()
        {
            Reviews = new ObservableCollection<Review>();
        }

        // Virtual method to get the creator of the item (e.g., author, director)
        public virtual string GetCreator()
        {
            return "Unknown Creator";
        }

        // Virtual method to display item details
        public virtual string GetDetails()
        {
            return $"{Title}, ISBN: {ISBN}, Rating: {Rating}, Published by: {Publisher} on {PublicationDate.ToShortDateString()}, Description: {Description}, Creator: {GetCreator()}, Total Copies: {TotalCopies}, Available Copies: {AvailableCopies}";
        }

        // Method to display the item type as a string
        public string GetItemTypeName()
        {
            return GetType().Name;
        }

        // Method to add a review to the item
        public void AddReview(Review review)
        {
            if (review != null)
            {
                Reviews.Add(review);
                Rating = AverageRating; // Update the overall rating
            }
        }

        // Method to generate a random ISBN for the item
        public string GenerateISBN()
        {
            ISBN = "ISBN-" + new Random().Next(1000000, 9999999);
            return ISBN;
        }
        public bool TrySetTotalCopies(int newTotalCopies)
        {
            int difference = newTotalCopies - _totalCopies;

            if (difference < 0 && AvailableCopies + difference < 0)
            {
                // Invalid operation
                return false;
            }

            // Update the available copies and total copies
            AvailableCopies += difference;
            _totalCopies = newTotalCopies;
            return true;
        }
        public string ValidateItemProperties(Item item)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(item.Title))
            {
                errors.Add("Title cannot be empty.");
            }

            if (!DateTime.TryParse(item.PublicationDate.ToString(), out _))
            {
                errors.Add("Publication Date must be a valid date.");
            }

            if (item.TotalCopies < 1)
            {
                errors.Add("Total Copies must be at least 1.");
            }

            if (item.AvailableCopies > item.TotalCopies)
            {
                errors.Add("Available Copies cannot exceed Total Copies.");
            }

            return string.Join(Environment.NewLine, errors);
        }

    }
}
