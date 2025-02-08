using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AmbulanceService.Models;

namespace AmbulanceService.Model
{
    public class User
    {
        [Key]
        public int Uid { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        [Column(TypeName = "varchar(255)")]
        public Role role { get; set; } = Role.USER;


    }
}
