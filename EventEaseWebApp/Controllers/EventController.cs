using EventEaseWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseWebApp.Controllers
{
    public class EventController : Controller
    {
        private readonly EventEaseDbContext _context;

        public EventController(EventEaseDbContext context)
        {
            _context = context;
        }

        // Replaced Old Index method with filtering enhanced version - Part 3 step 7
        public async Task<IActionResult>  Index( string searchType, int? VenueId, DateTime? startDate, DateTime? endDate)
        {
            var events =  _context.Event.
                Include(e => e.Venue).
                Include(e => e.EventType).
                AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
                events = events.Where(e => e.EventType.Name == searchType);

            if (VenueId.HasValue)
                events = events.Where(e => e.VenueId == VenueId);

            if (startDate.HasValue && endDate.HasValue)
                events = events.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);

            //Provide data for dropdown option
            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venue.ToList();


            return View(await events.ToListAsync());


        }

        // GET: Event/Details/5
        public async Task <IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var events = _context.Event.Include(e => e.Venue).FirstOrDefault(e => e.EventId == id);
            
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewData["Venues"] = _context.Venue.ToList();

            //Part 3 step 5: Load event types for dropdown
            ViewData["EventTypes"] = _context.EventType.ToList();


            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create( Event events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", events.VenueId);

            ViewData["EventTypeID"] = new SelectList(_context.Venue, "EventTypeID", "Name", events.EventTypeID);

            return View(events);
        }

        public async Task <IActionResult> Delete (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event.Include(e => e.Venue).FirstOrDefaultAsync (e => e.EventId == id);

            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @events = await _context.Event.FindAsync(id);

            //In detail this part of the code checks if the event exists in the database.
            if (@events == null) return NotFound();




            var validBooking = await _context.Booking.AnyAsync(b => b.EventId == id);
            if (validBooking)
            {
                TempData["Error"] = "This event cannot be deleted because it has associated bookings.";
                return RedirectToAction(nameof(Index));
            }

            try
            {


                _context.Event.Remove(@events);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Event successfully deleted!";
            }
            catch (Exception ex)

            {
                TempData["Error"] = "An error occurred while deleting the venue: ";
            }

            // Redirect to the Index action after deletion
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }

            ViewData["Venues"] = _context.Venue.ToList();

            //Part 3 Question (Step 5) ADDED: Load Event types for dropdown
            ViewData["EventTypes"] = _context.EventType.ToList();

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event events)
        {
            if (id != events.EventId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            { 
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event successfully updated";
                    return RedirectToAction(nameof(Index));
            }


            ViewData["Venues"] = _context.Venue.ToList();

            //Part 3 Question (Step 5) ADDED: Reload event types on failed edit submition
            ViewData["EventTypes"] = _context.EventType.ToList();


            return View(events);
        }



        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }

    }
}
