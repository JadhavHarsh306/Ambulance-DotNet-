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
    public class HospitalsController : ControllerBase
    {
        private readonly AmbulanceDBContext _context;

        public HospitalsController(AmbulanceDBContext context)
        {
            _context = context;
        }

        // GET: api/Hospitals
        [HttpGet("getHospital")]
        public async Task<ActionResult<IEnumerable<Hospital>>> Gethospitals()
        {
          if (_context.hospitals == null)
          {
              return NotFound();
          }
            return await _context.hospitals.ToListAsync();
        }

        // GET: api/Hospitals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hospital>> GetHospital(int id)
        {
          if (_context.hospitals == null)
          {
              return NotFound();
          }
            var hospital = await _context.hospitals.FindAsync(id);

            if (hospital == null)
            {
                return NotFound();
            }

            return hospital;
        }


        [HttpPut("updateHospital/{id}")]
        public async Task<IActionResult> PutHospital(int id, HospitalDTO hospitalDto)
        {
            var hospital = await _context.hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            hospital.Name = hospitalDto.Name;
            hospital.Address = hospitalDto.Address;
            hospital.Location = hospitalDto.Location;
            hospital.Phone = hospitalDto.Phone;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HospitalExists(id))
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


       
        [HttpPost("saveHospital")]
        public async Task<ActionResult<Hospital>> PostHospital(Hospital hospital)
        {
          if (_context.hospitals == null)
          {
              return Problem("Entity set 'AmbulanceDBContext.hospitals'  is null.");
          }
            _context.hospitals.Add(hospital);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHospital", new { id = hospital.Hid }, hospital);
        }

        // DELETE: api/Hospitals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            if (_context.hospitals == null)
            {
                return NotFound();
            }
            var hospital = await _context.hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }

            _context.hospitals.Remove(hospital);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HospitalExists(int id)
        {
            return (_context.hospitals?.Any(e => e.Hid == id)).GetValueOrDefault();
        }
    }
}
