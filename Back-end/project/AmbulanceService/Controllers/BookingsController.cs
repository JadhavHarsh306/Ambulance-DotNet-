using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmbulanceService.DAL;
using AmbulanceService.Models;
using AmbulanceService.DTO;

namespace AmbulanceService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AmbulanceDBContext _context;

        public BookingsController(AmbulanceDBContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> Getbookings()
        {
          if (_context.bookings == null)
          {
              return NotFound();
          }
            return await _context.bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
          if (_context.bookings == null)
          {
              return NotFound();
          }
            var booking = await _context.bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Bid)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking(
     [FromQuery] int userId,
     [FromQuery] string pickupLocation,
     [FromQuery] string dropLocation)
        {
            if (string.IsNullOrWhiteSpace(pickupLocation))
            {
                return BadRequest("Invalid booking details.");
            }

            // Split pickupLocation into latitude and longitude
            var locationParts = pickupLocation.Split(',');
            if (locationParts.Length != 2 ||
                !double.TryParse(locationParts[0], out double pickupLatitude) ||
                !double.TryParse(locationParts[1], out double pickupLongitude))
            {
                return BadRequest("Invalid pickup location format. Use 'latitude,longitude'.");
            }

            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {
                return NotFound($"User not found for ID: {userId}");
            }

            var booking = new Booking
            {
                UserId = userId,
                PickupLocationString = $"{pickupLatitude},{pickupLongitude}",
                DropLocation = dropLocation,
                Status = BookingStatus.PENDING
            };

            _context.bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateBooking), new { id = booking.Bid }, booking);
        }



        // DELETE: api/Bookings/5
        [HttpDelete("deleteBooking/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return (_context.bookings?.Any(e => e.Bid == id)).GetValueOrDefault();
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingBookings()
        {
            var pendingBookings = await _context.bookings
                .Where(b => b.Status == BookingStatus.PENDING)
                .Select(b => new
                {
                    b.Bid,
                    b.UserId,
                    UserName = _context.users.Where(u => u.Uid == b.UserId).Select(u => u.Name).FirstOrDefault(),
                    Phone = _context.users.Where(u => u.Uid == b.UserId).Select(u => u.Phone).FirstOrDefault(), 
                    b.PickupLocationString,
                    b.DropLocation,
                    b.Status
                })
                .ToListAsync();

            if (!pendingBookings.Any())
            {
                Console.WriteLine("No pending bookings found.");
                return NoContent();
            }

            Console.WriteLine("Pending bookings: " + pendingBookings);
            return Ok(pendingBookings);
        }
        [HttpPost("accept/{bookingId}/{driverId}")]
        public async Task<IActionResult> AcceptBooking(int bookingId, int driverId)
        {
            // Find the ambulance assigned to the given driver
            var ambulance = await _context.ambulances
                .Where(a => a.Did == driverId)
                .Select(a => a.AmbulanceId)
                .FirstOrDefaultAsync();

            if (ambulance == 0)  
            {
                return BadRequest("No ambulance assigned to this driver.");
            }

            var booking = await _context.bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return BadRequest("Booking not found.");
            }

            
            booking.Status = BookingStatus.ACCEPTED;
            booking.AmbulanceId = ambulance;

            _context.bookings.Update(booking);
            await _context.SaveChangesAsync();

            return Ok("Booking accepted successfully!");
        }

        [HttpPost("reject/{bookingId}")]
        public async Task<IActionResult> RejectBooking(int bookingId)
        {
            var booking = await _context.bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return BadRequest("Booking not found.");
            }
            booking.Status = BookingStatus.CANCELED;

            _context.bookings.Update(booking);
            await _context.SaveChangesAsync();

            return Ok("Booking rejected successfully!");
        }


    }
}
