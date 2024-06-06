using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class DVD : PhysicalItem
    {
        public string Director { get; set; }
        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }
        public string Language { get; set; }
        public List<string> Subtitles { get; set; }
        public string Studio { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<string> Cast { get; set; }

        // Constructor to initialize CopiesByLocation, Subtitles, and Cast
        public DVD()
        {
            CopiesByLocation = new Dictionary<string, int>();
            Subtitles = new List<string>();
            Cast = new List<string>();
        }

        // Override method to get the creator of the DVD
        public override string GetCreator()
        {
            return Director;
        }

        // Override method to display DVD details
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Director: {Director}, Genre: {Genre}, Duration: {Duration}, Language: {Language}, Studio: {Studio}, Release Date: {ReleaseDate.ToShortDateString()}, Subtitles: {string.Join(", ", Subtitles)}, Cast: {string.Join(", ", Cast)}";
        }
    }
}