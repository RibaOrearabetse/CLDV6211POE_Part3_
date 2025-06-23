using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace EventEaseWebApp.Models
{
    public class Venue
    {
        [Key]
       public int VenueId { get; set; }
        [Required]
        public string VenueName { get; set; }
        [Required]
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }
        
        [NotMapped]
        public IFormFile ? ImageFile { get; set; }

        public List<Event> Events { get; set; } = new();
    }
}
