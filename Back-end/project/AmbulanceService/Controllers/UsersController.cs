using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmbulanceService.DAL;
using AmbulanceService.Model;
using AmbulanceService.DTO;
using AmbulanceService.Models;

namespace AmbulanceService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AmbulanceDBContext _context;

        public UsersController(AmbulanceDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet("getUser")]
        public async Task<ActionResult<IEnumerable<User>>> Getusers()
        {
          if (_context.users == null)
          {
              return NotFound();
          }
            return await _context.users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.users == null)
          {
              return NotFound();
          }
            var user = await _context.users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDto)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = userDto.Name;
            user.Phone = userDto.Phone;
            user.Email = userDto.Email;
            user.Address = userDto.Address;
            user.Password = userDto.Password;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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


        [HttpPost("registerUser")]
        public async Task<ActionResult<User>> RegisterUser(UserDTO userDto)
        {
            if (_context.users == null)
            {
                return Problem("Entity set 'AmbulanceDBContext.users' is null.");
            }

            // Check if email is already registered
            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email already registered" });
            }

            // Create new user without password hashing
            var user = new User
            {
                Name = userDto.Name,
                Phone = userDto.Phone,
                Email = userDto.Email,
                Address = userDto.Address,
                Password = userDto.Password // Store password as plain text
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Uid }, new { message = "User registered successfully" });
        }


        // DELETE: api/Users/5
        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.users == null)
            {
                return NotFound();
            }
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.users?.Any(e => e.Uid == id)).GetValueOrDefault();
        }

        [HttpPost("loginUser")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password && u.role==Role.USER);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { message = "User Login successful", userId = user.Uid,userName=user.Name });
        }

        [HttpPost("payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentDto)
        {
            if (paymentDto == null || paymentDto.BookingId == 0 || paymentDto.UserId == 0)
            {
                return BadRequest("Invalid payment data.");
            }

            // Ensure the booking exists
            var booking = await _context.bookings.FindAsync(paymentDto.BookingId);
            if (booking == null)
            {
                return BadRequest("Booking not found.");
            }

            // Create a new Payment entity from DTO
            var payment = new Payments
            {
                Amount = (decimal)paymentDto.Amount,
                PaymentStatus = paymentDto.PaymentStatus,
                BookingId = paymentDto.BookingId,
                UserId = paymentDto.UserId
            };

            _context.payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ProcessPayment), new { id = payment.Pid }, "Payment processed successfully!");
        }

    }
}

