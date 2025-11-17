namespace Reservations.Api.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}