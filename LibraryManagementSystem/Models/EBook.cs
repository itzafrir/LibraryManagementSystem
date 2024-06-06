using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class EBook : Item
    {
        public string Author { get; set; }
        public string FileFormat { get; set; } // e.g., PDF, EPUB, MOBI
        public double FileSize { get; set; } // in MB
        public string DownloadLink { get; set; }
        public List<string> Keywords { get; set; }

        // Constructor to initialize Keywords
        public EBook()
        {
            Keywords = new List<string>();
        }

        // Override method to get the creator of the ebook
        public override string GetCreator()
        {
            return Author;
        }

        // Override method to display ebook details
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Author: {Author}, File Format: {FileFormat}, File Size: {FileSize} MB, Download Link: {DownloadLink}, Keywords: {string.Join(", ", Keywords)}";
        }
    }
}