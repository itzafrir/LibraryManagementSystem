using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a general item in the library system, containing basic properties and methods common to all items.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Catalog Number (ISBN) for the item.
        /// </summary>
        public string ISBN { get; set; }

        /// <summary>
        /// Gets or sets the rating of the item.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the publication date of the item.
        /// </summary>
        [Required]
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the publisher of the item.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews associated with the item.
        /// </summary>
        public ObservableCollection<Review> Reviews { get; set; }

        private int _totalCopies;
        /// <summary>
        /// Gets or sets the total number of copies available in the library.
        /// Adjusting this property also updates the available copies accordingly.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the number of available copies of the item in the library.
        /// </summary>
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

        /// <summary>
        /// Gets the average rating of the item based on its reviews.
        /// </summary>
        public double AverageRating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                    return 0;
                return Reviews.Average(r => r.Rating);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        public Item()
        {
            Reviews = new ObservableCollection<Review>();
        }

        /// <summary>
        /// Gets the creator of the item (e.g., author, director).
        /// </summary>
        /// <returns>A string representing the creator of the item.</returns>
        public virtual string GetCreator()
        {
            return "Unknown Creator";
        }

        /// <summary>
        /// Gets a detailed string representation of the item's properties.
        /// </summary>
        /// <returns>A string containing all relevant details of the item.</returns>
        public virtual string GetDetails()
        {
            return $"{Title}, ISBN: {ISBN}, Rating: {Rating}, Published by: {Publisher} on {PublicationDate.ToShortDateString()}, Description: {Description}, Creator: {GetCreator()}, Total Copies: {TotalCopies}, Available Copies: {AvailableCopies}";
        }

        /// <summary>
        /// Gets the name of the item type as a string.
        /// </summary>
        /// <returns>A string representing the name of the item type.</returns>
        public string GetItemTypeName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Adds a review to the item and updates the item's rating.
        /// </summary>
        /// <param name="review">The review to add.</param>
        public void AddReview(Review review)
        {
            if (review != null)
            {
                Reviews.Add(review);
                Rating = AverageRating; // Update the overall rating
            }
        }

        /// <summary>
        /// Generates a random ISBN for the item.
        /// </summary>
        /// <returns>A string representing the newly generated ISBN.</returns>
        public string GenerateISBN()
        {
            ISBN = new Random().Next(1000000, 9999999).ToString();
            return ISBN;
        }

        /// <summary>
        /// Tries to set the total number of copies for the item. Ensures that available copies are not set to a negative value.
        /// </summary>
        /// <param name="newTotalCopies">The new total number of copies to set.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
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

        /// <summary>
        /// Validates the properties of the given item and returns a string containing any validation errors.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>A string containing any validation errors, or an empty string if no errors are found.</returns>
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