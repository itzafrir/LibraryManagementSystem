using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class CD : Item
    {
        public string Artist { get; set; }
        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }
        public int TrackCount { get; set; }
        public string Label { get; set; }
        public List<string> Tracks { get; set; }

        // Constructor to initialize CopiesByLocation and Tracks
        public CD()
        {
            Tracks = new List<string>();
        }

        // Override method to get the creator of the CD
        public override string GetCreator()
        {
            return Artist;
        }

        // Override method to display CD details
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Artist: {Artist}, Genre: {Genre}, Duration: {Duration}, Track Count";
        }
    }
}