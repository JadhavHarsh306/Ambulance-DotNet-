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
    public class AdminsController : ControllerBase
    {
        private readonly AmbulanceDBContext _context;

        public AdminsController(AmbulanceDBContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var admin = _context.admins.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password && u.Role==Role.ADMIN);
            if (admin == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { message = "Admin Login successful", userId = admin.Aid,userName=admin.Name });
        }
    }

}
