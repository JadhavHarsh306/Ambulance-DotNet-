using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmbulanceService.Models
{
    public class Ambulance
    {
        [Key]
        public int AmbulanceId { get; set; }

        public string LicensePlate { get; set; }

        [ForeignKey("Hospital")]
        public int Hid { get; set; }
        public Hospital Hospital { get; set; }

        [ForeignKey("Driver")]
        public int Did { get; set; }
        public Driver Driver { get; set; }
    }
}
