using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AmbulanceService.Models
{
    public class Admin
    {
        [Key]
        public int Aid { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Column(TypeName = "varchar(255)")]
        public Role Role { get; set; } = Role.ADMIN;
    }
}
