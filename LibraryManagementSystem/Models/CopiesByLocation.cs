using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class CopiesByLocation
    {
        [Key]
        public int Id { get; set; }
        public string Location { get; set; }
        public int Copies { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
