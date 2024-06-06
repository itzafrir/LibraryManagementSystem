using LibraryManagementSystem.Utilities.Enums;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public ItemType ItemType { get; set; }
        public double Rating { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public ICollection<CopiesByLocation> CopiesByLocations { get; set; }

        // Virtual method to get the creator of the item
        public virtual string GetCreator()
        {
            return "Unknown Creator";
        }

        // Method to display item details
        public virtual string GetDetails()
        {
            return $"{Title}, ISBN: {ISBN}, Rating: {Rating}, Published by: {Publisher} on {PublicationDate.ToShortDateString()}, Description: {Description}, Creator: {GetCreator()}";
        }
    }
}