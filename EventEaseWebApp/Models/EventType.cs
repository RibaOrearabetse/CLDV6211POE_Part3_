using EventEaseWebApp.Models;

namespace EventEaseWebApp.Models
{
    public class EventType
    {
        public int EventTypeID { get; set; }
        public string Name { get; set; }  
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }

}
