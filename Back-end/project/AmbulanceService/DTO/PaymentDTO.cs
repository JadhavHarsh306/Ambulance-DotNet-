using System.Text.Json.Serialization;
using AmbulanceService.Models;

namespace AmbulanceService.DTO
{
    public class PaymentDTO
    {
        public double? Amount { get; set; } // Payment Amount

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; } // Enum for Payment Status (PENDING, COMPLETED)
        public int BookingId { get; set; } // Foreign Key for Booking
        public int UserId { get; set; } // Foreign Key for User
    }

}
