using System.ComponentModel.DataAnnotations;
using EventEaseWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseWebApp.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]

        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "The Venue field is required.")]
        public int VenueId { get; set; }  // non-nullable, required foreign key



        public Venue? Venue { get; set; } = null;

       public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public int? EventTypeID { get; set; } // step 4

        public EventType? EventType { get; set; } // step 4
    
    }
}
