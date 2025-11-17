namespace Reservations.Api.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}