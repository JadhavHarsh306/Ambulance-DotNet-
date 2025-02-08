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
    public class DriversController : ControllerBase
    {
        private readonly AmbulanceDBContext _context;

        public DriversController(AmbulanceDBContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [HttpGet("getDriver")]
        public async Task<ActionResult<IEnumerable<Driver>>> Getdrivers()
        {
          if (_context.drivers == null)
          {
              return NotFound();
          }
            return await _context.drivers.ToListAsync();
        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
          if (_context.drivers == null)
          {
              return NotFound();
          }
            var driver = await _context.drivers.FindAsync(id);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        [HttpPut("updateDriver/{id}")]
        public async Task<IActionResult> PutDriver(int id, updateDriver driverDto)
        {
            var driver = await _context.drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            // Update only the necessary fields
            driver.Name = driverDto.Name;
            driver.Phone = driverDto.Phone;
            driver.License = driverDto.License;
            driver.Experience = driverDto.Experience;
   

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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


        [HttpPost("driverRegister")]
        public async Task<ActionResult<Driver>> PostDriver(DriverDTO driverDto)
        {
            if (_context.drivers == null)
            {
                return Problem("Entity set 'AmbulanceDBContext.drivers' is null.");
            }

            var driver = new Driver
            {
                Name = driverDto.Name,
                Phone = driverDto.Phone,
                Email = driverDto.Email,
                License = driverDto.License,
                Experience = driverDto.Experience,
                Address = driverDto.Address,
                Password = driverDto.Password
            };

            _context.drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { id = driver.Did }, driver);
        }



        // DELETE: api/Drivers/5
        [HttpDelete("deleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            if (_context.drivers == null)
            {
                return NotFound();
            }
            var driver = await _context.drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverExists(int id)
        {
            return (_context.drivers?.Any(e => e.Did == id)).GetValueOrDefault();
        }

        [HttpPost("loginDriver")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var driver = _context.drivers.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password && u.Role==Role.DRIVER);
            if (driver == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { message = "Driver Login successful", userId = driver.Did,userName=driver.Name });
        }
    }

   
}
