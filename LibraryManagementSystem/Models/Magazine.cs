using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a magazine item in the library system, extending the base item class with specific magazine-related properties.
    /// </summary>
    public class Magazine : Item
    {
        /// <summary>
        /// Gets or sets the editor of the magazine.
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// Gets or sets the issue number of the magazine.
        /// </summary>
        public int IssueNumber { get; set; }

        /// <summary>
        /// Gets or sets the genre of the magazine.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the publication frequency of the magazine (e.g., Weekly, Monthly).
        /// </summary>
        public string Frequency { get; set; }

        /// <summary>
        /// Gets or sets the list of articles included in the magazine.
        /// </summary>
        public List<string> Articles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Magazine"/> class.
        /// </summary>
        public Magazine()
        {
            Articles = new List<string>();
        }

        /// <summary>
        /// Gets the creator of the magazine, which is the editor.
        /// </summary>
        /// <returns>A string representing the editor of the magazine.</returns>
        public override string GetCreator()
        {
            return Editor;
        }

        /// <summary>
        /// Gets a detailed string representation of the magazine's properties.
        /// </summary>
        /// <returns>A string containing all relevant details of the magazine.</returns>
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Editor: {Editor}, Issue Number: {IssueNumber}, Genre: {Genre}, Frequency: {Frequency}, Articles: {string.Join(", ", Articles)}";
        }
    }
}