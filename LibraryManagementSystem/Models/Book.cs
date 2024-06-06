using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class Book : Item
    {
        public string Author { get; set; }
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public string Language { get; set; }
        public string Format { get; set; } // e.g., Hardcover, Paperback, etc.
        public string Dimensions { get; set; } // e.g., 6 x 9 inches
        public string Series { get; set; }
        public int Edition { get; set; }
        public List<string> Keywords { get; set; }

        // Constructor to initialize CopiesByLocation and Keywords
        public Book()
        {
            Keywords = new List<string>();
        }

        // Override method to get the creator of the book
        public override string GetCreator()
        {
            return Author;
        }

        // Override method to display book details
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Author: {Author}, Genre: {Genre}, Page Count: {PageCount}, Language: {Language}, Format: {Format}, Dimensions: {Dimensions}, Series: {Series}, Edition: {Edition}, Keywords: {string.Join(", ", Keywords)}";
        }
    }
}