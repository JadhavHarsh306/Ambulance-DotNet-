using AmbulanceService.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmbulanceService.Models
{
    public class Booking
    {
        [Key]
        public int Bid { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Ambulance")]
        public int? AmbulanceId { get; set; }

        public string PickupLocationString { get; set; } // Stores "latitude,longitude" as string
        public string DropLocation { get; set; }

        public BookingStatus Status { get; set; }
    }

}
