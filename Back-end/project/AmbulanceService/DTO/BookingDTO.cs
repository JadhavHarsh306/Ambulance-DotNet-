namespace AmbulanceService.DTO
{
    public class BookingDTO
    {
        public int UserId { get; set; }
        public string? PickupLocation { get; set; } 
        public string? DropLocation { get; set; }
    }

}
