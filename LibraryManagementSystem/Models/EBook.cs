using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents an eBook item in the library system, extending the base item class with specific eBook-related properties.
    /// </summary>
    public class EBook : Item
    {
        /// <summary>
        /// Gets or sets the author of the eBook.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the file format of the eBook (e.g., PDF, EPUB, MOBI).
        /// </summary>
        public string FileFormat { get; set; }

        /// <summary>
        /// Gets or sets the file size of the eBook in megabytes (MB).
        /// </summary>
        public double FileSize { get; set; }

        /// <summary>
        /// Gets or sets the download link for the eBook.
        /// </summary>
        public string DownloadLink { get; set; }

        /// <summary>
        /// Gets or sets the list of keywords associated with the eBook.
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EBook"/> class.
        /// </summary>
        public EBook()
        {
            Keywords = new List<string>();
        }

        /// <summary>
        /// Gets the creator of the eBook, which is the author.
        /// </summary>
        /// <returns>A string representing the author of the eBook.</returns>
        public override string GetCreator()
        {
            return Author;
        }

        /// <summary>
        /// Gets a detailed string representation of the eBook's properties.
        /// </summary>
        /// <returns>A string containing all relevant details of the eBook.</returns>
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Author: {Author}, File Format: {FileFormat}, File Size: {FileSize} MB, Download Link: {DownloadLink}, Keywords: {string.Join(", ", Keywords)}";
        }
    }
}