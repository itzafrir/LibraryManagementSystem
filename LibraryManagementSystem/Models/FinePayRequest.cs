using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class FinePayRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FineId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }

        // Navigation properties
        public virtual Fine Fine { get; set; }
        public virtual User User { get; set; }

        public string GetRequestDetails()
        {
            return $"Request ID: {Id}, Fine ID: {FineId}, User ID: {UserId}, Request Date: {RequestDate.ToShortDateString()}";
        }
    }
}
