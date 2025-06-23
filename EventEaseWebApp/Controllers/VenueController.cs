using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEaseWebApp.Models;
using System.Linq.Expressions;

namespace EventEaseWebApp.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventEaseDbContext _context;


        public VenueController(EventEaseDbContext context)
        {
            _context = context;

        }
        /* private readonly BlobContainerClient _containerClient;

         public VenueController(IConfiguration configuration)
         {
             var connectionString = configuration["AzureBlobStorage:ConnectionString"];
             var containerName = configuration["AzureBlobStorage:ContainerName"];

             if (string.IsNullOrEmpty(containerName))
             {
                 throw new ArgumentNullException(nameof(containerName), "Azure Blob container name is not set in configuration.");
             }

             _containerClient = new BlobContainerClient(connectionString, containerName);
         }*/

        // GET: Venue
        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venue.ToListAsync();
            return View(venues);
        }

        // GET: Venue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                if (venue.ImageFile != null)
                {
                    var blobURL = await UploadImageToBlobAsync(venue.ImageFile);

                    venue.ImageUrl = blobURL;
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Venue successfully Created";
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }


        // GET: Venue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.Include(v => v.Events).FirstOrDefaultAsync(v => v.VenueId == id);


            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FirstOrDefaultAsync(v => v.VenueId == id);


            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            var hasBooking = await _context.Booking.AnyAsync(b => b.VenueId == id);

            if (hasBooking)
            {
                TempData["Error"] = "You cannot delete this venue. It has active bookings.";
                return RedirectToAction(nameof(Index));
            }
            try
            {


                _context.Venue.Remove(venue);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venu successfully deleted";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occured while deleting Venue";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {

            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var venueExisting = await _context.Venue.AsNoTracking().FirstOrDefaultAsync(v => v.VenueId == id);

                    if (venueExisting == null)

                        return NotFound();

                    if (venue.ImageFile != null)
                    {
                        var urlBlob = await UploadImageToBlobAsync(venue.ImageFile);
                        venue.ImageUrl = urlBlob;
                    }
                    else
                    {
                        venue.ImageUrl = venueExisting.ImageUrl;
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Successfully updated Venue";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;

                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);

        }
        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }


        /*
private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
{
    
    var connectionString = "DefaultEndpointsProtocol=https;AccountName=blobstorageeventeaze;AccountKey=Zkb+v2LqOmw/6dp8Kk5IfKzd0UjNu0g7h6aYy0kAP6fXUP9+MYonHVuFSkMNbVjrnhIlbqrPlNNO+AStvGMdGg==;EndpointSuffix=core.windows.net";
    var containerName = "cldv6211poe";

    var blobServiceClient = new BlobServiceClient(connectionString);
    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

    var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
    {
        ContentType = imageFile.ContentType
    };

    using (var stream = imageFile.OpenReadStream())
    {
        await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });
    }

    return blobClient.Uri.ToString();
    
}
*/

    }
}
