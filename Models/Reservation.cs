namespace Reservations.Api.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Responsible { get; set; } = null!;
        public bool CoffeeRequested { get; set; }
        public CoffeeOption? CoffeeOption { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}