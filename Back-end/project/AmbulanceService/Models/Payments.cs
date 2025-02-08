using AmbulanceService.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmbulanceService.Models
{
    public enum PaymentStatus
    {
        PENDING,
        COMPLETED
    }

    public class Payments
    {
        [Key]
        public int Pid { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
