using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmbulanceService.Models
{
    public class Driver
    {
        [Key]
        public int Did { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string License { get; set; }
        public int Experience { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public ICollection<Ambulance> AssignedAmbulance { get; set; }

        [Column(TypeName = "varchar(255)")]
        public Role Role { get; set; } = Role.DRIVER;


    }
}
