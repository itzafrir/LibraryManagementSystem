using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a CD item in the library system, extending the base item class with specific CD-related properties.
    /// </summary>
    public class CD : Item
    {
        /// <summary>
        /// Gets or sets the artist of the CD.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the genre of the CD.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the CD.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the total number of tracks on the CD.
        /// </summary>
        public int TrackCount { get; set; }

        /// <summary>
        /// Gets or sets the record label that produced the CD.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the list of track names on the CD.
        /// </summary>
        public List<string> Tracks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CD"/> class.
        /// </summary>
        public CD()
        {
            Tracks = new List<string>();
        }

        /// <summary>
        /// Gets the creator of the CD, which is the artist.
        /// </summary>
        /// <returns>A string representing the artist of the CD.</returns>
        public override string GetCreator()
        {
            return Artist;
        }

        /// <summary>
        /// Gets a detailed string representation of the CD's properties.
        /// </summary>
        /// <returns>A string containing all relevant details of the CD.</returns>
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Artist: {Artist}, Genre: {Genre}, Duration: {Duration}, Track Count: {TrackCount}, Label: {Label}, Tracks: {string.Join(", ", Tracks)}";
        }
    }
}
