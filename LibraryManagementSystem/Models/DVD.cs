using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a DVD item in the library system, extending the base item class with specific DVD-related properties.
    /// </summary>
    public class DVD : Item
    {
        /// <summary>
        /// Gets or sets the director of the DVD.
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Gets or sets the genre of the DVD.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the DVD.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the language of the DVD.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the list of available subtitles for the DVD.
        /// </summary>
        public List<string> Subtitles { get; set; }

        /// <summary>
        /// Gets or sets the studio that produced the DVD.
        /// </summary>
        public string Studio { get; set; }

        /// <summary>
        /// Gets or sets the list of cast members in the DVD.
        /// </summary>
        public List<string> Cast { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DVD"/> class.
        /// </summary>
        public DVD()
        {
            Subtitles = new List<string>();
            Cast = new List<string>();
        }

        /// <summary>
        /// Gets the creator of the DVD, which is the director.
        /// </summary>
        /// <returns>A string representing the director of the DVD.</returns>
        public override string GetCreator()
        {
            return Director;
        }

        /// <summary>
        /// Gets a detailed string representation of the DVD's properties.
        /// </summary>
        /// <returns>A string containing all relevant details of the DVD.</returns>
        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Director: {Director}, Genre: {Genre}, Duration: {Duration}, Language: {Language}, Studio: {Studio}, Subtitles: {string.Join(", ", Subtitles)}, Cast: {string.Join(", ", Cast)}";
        }
    }
}