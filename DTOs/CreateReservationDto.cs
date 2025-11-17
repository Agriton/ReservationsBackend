namespace Reservations.Api.Dtos
{
    public class CoffeeDto
    {
        public int Quantity { get; set; }
        public string? Description { get; set; }
    }
    public class CreateReservationDto
    {
        public int RoomId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Responsible { get; set; } = null!;
        public bool CoffeeRequested { get; set; }
        public CoffeeDto? CoffeeOption { get; set; }
    }
}