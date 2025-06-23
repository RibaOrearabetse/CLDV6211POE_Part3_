using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventEaseWebApp.Models;

namespace EventEaseWebApp.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey ("Event")]
        public int EventId { get; set; }
        public Event? Event { get; set; }

        [ForeignKey ("Venue")]
        public int VenueId { get; set; }

        public Venue? Venue { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

       
    }
}
