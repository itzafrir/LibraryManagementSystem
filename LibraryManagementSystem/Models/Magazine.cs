using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class Magazine : Item
    {
        public string Editor { get; set; }
        public int IssueNumber { get; set; }
        public string Genre { get; set; }
        public string Frequency { get; set; } // e.g., Weekly, Monthly
        public List<string> Articles { get; set; }

        // Constructor to initialize CopiesByLocation and Articles
        public Magazine()
        {
            Articles = new List<string>();
        }

        // Override method to get the creator of the magazine
        public override string GetCreator()
        {
            return Editor;
        }

        // Override method to display magazine details
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Editor: {Editor}, Issue Number: {IssueNumber}, Genre: {Genre}, Frequency: {Frequency}, Articles: {string.Join(", ", Articles)}";
        }
    }
}