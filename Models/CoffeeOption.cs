namespace Reservations.Api.Models
{
    public class CoffeeOption
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Description { get; set; }
    }
}