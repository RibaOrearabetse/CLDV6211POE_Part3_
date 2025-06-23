using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEaseWebApp.Models;
using Microsoft.AspNetCore.Mvc.Routing;


namespace EventEaseWebApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventEaseDbContext _context;

        public BookingController(EventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index(string searchBox)
        {
            // Get all bookings and include related Event and Venue data from the database
            var bookings = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable(); // Make it a query so we can filter it later

            // If the user typed something in the search box, filter the bookings
            if (!string.IsNullOrEmpty(searchBox))
            {
                // Search by Booking ID or Event Name
                bookings = bookings.Where(b =>
                    b.BookingId.ToString().Contains(searchBox) ||
                    b.Event.EventName.Contains(searchBox));
            }

            return View(await bookings.ToListAsync());
        }


        // GET: Booking/Create
        public  IActionResult Create()
        {
            
            
                ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName");
                ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName");

            return View();
        }





        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate")] Booking bookings)
        {
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName");
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName");
            // Get the event linked to this bookings
            var EventSelected = await _context.Event
                    .FirstOrDefaultAsync(e => e.EventId == bookings.EventId);

            if (EventSelected == null)
            {
                ModelState.AddModelError("", "Event selected  not found.");
                // Populate ViewBag with Event and Venue data
                return View(bookings);
            }

            
            if (ModelState.IsValid)
            {
                // Check for existing bookings on the same date and venue
                var isVenueBooked = await _context.Booking

            .AnyAsync(b =>
                b.VenueId == bookings.VenueId &&

                b.BookingDate.Date == bookings.BookingDate.Date);

                // bool isVenueAlreadyBooked = existingBookings.Any(b =>
                //  b.Event != null && b.Event.EventDate.Date == eventDateOnly);

                if (isVenueBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked on the selected date.");

                    

                    return View(bookings);
                }

                



                _context.Add(bookings);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Booking successfully created!";
                return RedirectToAction(nameof(Index));

            }
            // Populate ViewBag with Event and Venue data
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName");
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName");

            return View(bookings);


        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking
    .Include(b => b.Event)
    .Include(b => b.Venue)
    .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking
       .Include(b => b.Event)
       .Include(b => b.Venue)
       .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            ViewBag.CurrentVenueName = booking.Venue?.VenueName;

            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {

            if (id != booking.BookingId)
            {
                return NotFound();
            }


            //var selectedEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventId == booking.EventId);


            if (ModelState.IsValid)
            {


                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Booking updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index");
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(b => b.BookingId == id);
        }


    }
}

