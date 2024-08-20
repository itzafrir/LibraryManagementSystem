using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a book item in the library system, extending the base item class with specific book-related properties.
    /// </summary>
    public class Book : Item
    {
        /// <summary>
        /// Gets or sets the author of the book.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the genre of the book.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages in the book.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the language in which the book is written.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the format of the book (e.g., Hardcover, Paperback).
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the dimensions of the book (e.g., 6 x 9 inches).
        /// </summary>
        public string Dimensions { get; set; }

        /// <summary>
        /// Gets or sets the series the book belongs to, if any.
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the edition number of the book.
        /// </summary>
        public int Edition { get; set; }

        /// <summary>
        /// Gets or sets the list of keywords associated with the book.
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        public Book()
        {
            Keywords = new List<string>();
        }

        /// <summary>
        /// Gets the creator of the book, which is the author.
        /// </summary>
        /// <returns>A string representing the author of the book.</returns>
        public override string GetCreator()
        {
            return Author;
        }

        /// <summary>
        /// Gets a detailed string representation of the book's properties.
        /// </summary>
        /// <returns>A string containing all relevant details of the book.</returns>
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Author: {Author}, Genre: {Genre}, Page Count: {PageCount}, Language: {Language}, Format: {Format}, Dimensions: {Dimensions}, Series: {Series}, Edition: {Edition}, Keywords: {string.Join(", ", Keywords)}";
        }
    }
}